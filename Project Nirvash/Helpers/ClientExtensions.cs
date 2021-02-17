using HeyRed.Mime;
using Newtonsoft.Json;
using Project_Nirvash.Core.Helpers;
using Project_Nirvash.Exceptions;
using Project_Nirvash.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Project_Nirvash.Helpers
{
    /// <summary>
    /// ClientExtensions class
    /// </summary>
    public class ClientExtensions : IDisposable
    {
        public Profile AccountDetails { get; set; } = new Profile();

        private const string API_URL = "https://web.spaggiari.eu/rest/v1/";
        private const string API_KEY = "Tg1NWEwNGIgIC0K";

        private const string LOGIN_URL = "auth/login";
        private const string ABSENCES_URL = "students/{0}/absences/details";
        private const string AGENDA_URL = "students/{0}/agenda/all/{1}/{2}";
        private const string NOTICEBOARD_URL = "students/{0}/noticeboard";
        private const string NOTICEBOARD_READ_URL = "students/{0}/noticeboard/read/{1}/{2}/1";
        private const string NOTICEBOARD_ATTACHMENT_URL = "students/{0}/noticeboard/attach/{1}/{2}/{3}";
        private const string DIDACTICS_URL = "students/{0}/didactics";
        private const string DIDACTICS_ATTACHMENT_URL = "students/{0}/didactics/item/{1}";
        private const string GRADES_URL = "students/{0}/grades";
        private const string NOTES_URL = "students/{0}/notes/all";
        private const string SUBJECTS_URL = "students/{0}/subjects";
        private const string LESSONS_URL = "students/{0}/lessons/{1}/{2}";

        private readonly HttpClient httpClient;
        private AuthResponse authResponse;

        private DispatcherTimer sessionTimeout;

        private bool isDisposed;

        public ClientExtensions()
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(API_URL)
            };

            httpClient.DefaultRequestHeaders.Add("User-Agent", "CVVS/std");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;

            if (disposing)
            {
                httpClient.Dispose();
            }

            isDisposed = true;
        }

        public async Task<bool> AuthenticateAndInitDataAsync(UserCredentials userCredentials, bool roamResult)
        {
            if(await AuthenticateUserAsync(userCredentials))
            {
                await GetSubjectsAsync();

                Task[] tasks = new Task[]
                {
                    GetAgendaAsync(roamResult),
                    GetLessonsAsync(),
                    GetNoticeboardAsync(roamResult),
                    GetAbsencesAsync(roamResult),
                    GetDidacticsAsync(roamResult),
                    GetGradesAsync(roamResult),
                    GetNotesAsync(roamResult)
                };

                await Task.WhenAll(tasks);

                return true;
            }

            return false;
        }

        public async Task<bool> AuthenticateUserAsync(UserCredentials userCredentials)
        {
            HttpContent httpContent = await SendRequestAsync(HttpMethod.Post, LOGIN_URL, userCredentials);
            authResponse = JsonConvert.DeserializeObject<AuthResponse>(await httpContent.ReadAsStringAsync());

            AccountDetails.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Format("{0} {1}", authResponse.FirstName, authResponse.LastName).ToLower()); 

            if (authResponse.Token != null)
            {
                sessionTimeout = new DispatcherTimer
                {
                    Interval = authResponse.Expiration - DateTime.Now
                };

                sessionTimeout.Tick += (s, o) =>
                {
                    // Refresh authentication token
                    Singleton<ClientExtensions>.Instance.AuthenticateUserAsync(userCredentials);
                };
                sessionTimeout.Start();
            }

            return authResponse.Token != null;
        }

        public void DisconnectUser()
        {
            sessionTimeout.Stop();

            authResponse = new AuthResponse();

            AccountDetails.AbsenceEvents.Clear();
            AccountDetails.AgendaEvents.Clear();
            AccountDetails.DidacticsItems.Clear();
            AccountDetails.Grades.Clear();
            AccountDetails.Lessons.Clear();
            AccountDetails.Notes.Clear();
            AccountDetails.NoticeboardItems.Clear();
            AccountDetails.Subjects.Clear();
        }

        public async Task<AbsenceResponse> GetAbsencesAsync(bool roamResult)
        {
            string requestUri = string.Format(ABSENCES_URL, authResponse.UserID);

            HttpContent httpContent = await SendRequestAsync(HttpMethod.Get, requestUri, null);
            AbsenceResponse response = JsonConvert.DeserializeObject<AbsenceResponse>(await httpContent.ReadAsStringAsync());

            AccountDetails.AbsenceEvents.Clear();
            AccountDetails.AbsenceEvents.AddRange(response.Events);

            if (roamResult)
            {
                ResourceExtensions.StoreRoamingObject(BackgroundTaskExtensions.EVENTS, 0);
            }

            return response;
        }

        public async Task<AgendaResponse> GetAgendaAsync(bool roamResult)
        {
            string startingDate = DateTimeExtensions.StartingSchoolDate.ToString("yyyyMMdd");
            string endingDate = DateTimeExtensions.EndingSchoolDate.ToString("yyyyMMdd");

            string requestUri = string.Format(AGENDA_URL, authResponse.UserID, startingDate, endingDate);

            HttpContent httpContent = await SendRequestAsync(HttpMethod.Get, requestUri, null);
            AgendaResponse response = JsonConvert.DeserializeObject<AgendaResponse>(await httpContent.ReadAsStringAsync());

            AccountDetails.AgendaEvents.Clear();
            AccountDetails.AgendaEvents.AddRange(response.Appointments);

            if (roamResult)
            {
                ResourceExtensions.StoreRoamingObject(BackgroundTaskExtensions.APPOINTMENTS, response.Appointments.Count);
            }

            return response;
        }

        public async Task<NoticeboardResponse> GetNoticeboardAsync(bool roamResult)
        {
            string requestUri = string.Format(NOTICEBOARD_URL, authResponse.UserID);

            HttpContent httpContent = await SendRequestAsync(HttpMethod.Get, requestUri, null);
            NoticeboardResponse response = JsonConvert.DeserializeObject<NoticeboardResponse>(await httpContent.ReadAsStringAsync());

            AccountDetails.NoticeboardItems.Clear();
            AccountDetails.NoticeboardItems = new ObservableCollection<Item>(response.Items);

            if (roamResult)
            {
                ResourceExtensions.StoreRoamingObject(BackgroundTaskExtensions.ITEMS, response.Items.Count);
            }

            return response;
        }

        public async Task<bool> SetNoticeboardReadStatusAsync(string evtCode, int pubId)
        {
            string requestUri = string.Format(NOTICEBOARD_READ_URL, authResponse.UserID, evtCode, pubId);

            await SendRequestAsync(HttpMethod.Post, requestUri, null);

            return true;
        }

        public async Task<StorageFile> GetNoticeboardItemAttachmentAsync(string evtCode, int pubId, int attachNum)
        {
            string requestUri = string.Format(NOTICEBOARD_ATTACHMENT_URL, authResponse.UserID, evtCode, pubId, attachNum);

            HttpContent httpContent = await SendRequestAsync(HttpMethod.Get, requestUri, null);

            StorageFile file = await DownloadsFolder.CreateFileAsync(httpContent.Headers.ContentDisposition.FileName, CreationCollisionOption.GenerateUniqueName);
            await FileIO.WriteBytesAsync(file, await httpContent.ReadAsByteArrayAsync());

            return file;
        }

        public async Task<DidacticsResponse> GetDidacticsAsync(bool roamResult)
        {
            string requestUri = string.Format(DIDACTICS_URL, authResponse.UserID);

            HttpContent httpContent = await SendRequestAsync(HttpMethod.Get, requestUri, null);
            DidacticsResponse response = JsonConvert.DeserializeObject<DidacticsResponse>(await httpContent.ReadAsStringAsync());

            AccountDetails.DidacticsItems.Clear();
            AccountDetails.DidacticsItems.AddRange(PrepareDidacticts(response.Didactics));

            if (roamResult)
            {
                ResourceExtensions.StoreRoamingObject(BackgroundTaskExtensions.DIDACTICS, response.Didactics.Count);
            }

            return response;
        }

        private List<Didactict> PrepareDidacticts(List<Didactict> didacticts)
        {
            foreach(Didactict didactict in didacticts)
            {
                Subject subject = Subject.GetSubjectFromTeacher(AccountDetails.Subjects, new Teacher(didactict.TeacherID, didactict.TeacherName));
                didactict.SubjectName = subject == null ? "EmptySubject".GetLocalized() : subject.Description;

                // didactict.Contents = didactict.Folders.SelectMany(item => item.Contents).ToList();

                foreach (Folder folder in didactict.Folders)
                {
                    foreach (Content content in folder.Contents)
                    {
                        if (string.IsNullOrWhiteSpace(content.Name))
                        {
                            content.Name = folder.Name;
                        }
                        content.Folder = folder.Name;
                        didactict.Contents.Add(content);
                    }
                }

                didactict.Contents = didactict.Contents.OrderByDescending(content => content.Name).ToList();
            }
            return didacticts;
        }

        public async Task<GradesResponse> GetGradesAsync(bool roamResult)
        {
            string requestUri = string.Format(GRADES_URL, authResponse.UserID);

            HttpContent httpContent = await SendRequestAsync(HttpMethod.Get, requestUri, null);
            GradesResponse response = JsonConvert.DeserializeObject<GradesResponse>(await httpContent.ReadAsStringAsync());

            AccountDetails.Grades.Clear();
            AccountDetails.Grades.AddRange(response.Grades);

            PrepareGrades(response.Grades);

            if (roamResult)
            {
                ResourceExtensions.StoreRoamingObject(BackgroundTaskExtensions.GRADES, response.Grades.Count);
            }

            return response;
        }

        private void PrepareGrades(List<Grade> grades)
        {
            Parallel.ForEach(AccountDetails.Subjects, subject => {
                subject.FirstPeriodGrades = grades.Where(item => item.SubjectID.Equals(subject.ID) && item.Date.Month >= 9 && item.Date.Month <= 12).ToList();
                subject.SecondPeriodGrades = grades.Where(item => item.SubjectID.Equals(subject.ID) && item.Date.Month >= 1 && item.Date.Month <= 6).ToList();
            });
        }

        public async Task<NotesResponse> GetNotesAsync(bool roamResult)
        {
            string requestUri = string.Format(NOTES_URL, authResponse.UserID);

            HttpContent httpContent = await SendRequestAsync(HttpMethod.Get, requestUri, null);
            NotesResponse response = JsonConvert.DeserializeObject<NotesResponse>(await httpContent.ReadAsStringAsync());

            AccountDetails.Notes.Clear();
            AccountDetails.Notes.AddRange(response.NTCL);
            AccountDetails.Notes.AddRange(response.NTST);
            AccountDetails.Notes.AddRange(response.NTTE);
            AccountDetails.Notes.AddRange(response.NTWN);

            if (roamResult)
            {
                ResourceExtensions.StoreRoamingObject(BackgroundTaskExtensions.NOTES, AccountDetails.Notes.Count);
            }

            return response;
        }

        public async Task<SubjectsResponse> GetSubjectsAsync()
        {
            string requestUri = string.Format(SUBJECTS_URL, authResponse.UserID);

            HttpContent httpContent = await SendRequestAsync(HttpMethod.Get, requestUri, null);
            SubjectsResponse response = JsonConvert.DeserializeObject<SubjectsResponse>(await httpContent.ReadAsStringAsync());

            AccountDetails.Subjects.Clear();
            AccountDetails.Subjects.AddRange(response.Subjects);

            return response;
        }

        public async Task<LessonsResponse> GetLessonsAsync()
        {
            string startingDate = DateTimeExtensions.StartingSchoolDate.ToString("yyyyMMdd");
            string endingDate = DateTimeExtensions.EndingSchoolDate.ToString("yyyyMMdd");

            string requestUri = string.Format(LESSONS_URL, authResponse.UserID, startingDate, endingDate);

            HttpContent httpContent = await SendRequestAsync(HttpMethod.Get, requestUri, null);
            LessonsResponse response = JsonConvert.DeserializeObject<LessonsResponse>(await httpContent.ReadAsStringAsync());

            AccountDetails.Lessons.Clear();
            AccountDetails.Lessons.AddRange(response.Lessons);

            return response;
        }

        public async Task<StorageFile> GetDidacticsItemAttachmentAsync(Content content)
        {
            string requestUri = string.Format(DIDACTICS_ATTACHMENT_URL, authResponse.UserID, content.ID);

            HttpContent httpContent = await SendRequestAsync(HttpMethod.Get, requestUri, null);

            StorageFile file;
            if (httpContent.Headers.ContentDisposition == null)
            {
                if (httpContent.Headers.ContentType != null)
                {
                    file = await DownloadsFolder.CreateFileAsync(string.Format("{0}.{1}", content.Name, MimeTypesMap.GetExtension(httpContent.Headers.ContentType.MediaType)), CreationCollisionOption.GenerateUniqueName);
                }
                else
                {
                    file = await DownloadsFolder.CreateFileAsync(content.Name);
                }
            }
            else
            {
                file = await DownloadsFolder.CreateFileAsync(httpContent.Headers.ContentDisposition.FileName, CreationCollisionOption.GenerateUniqueName);
            }
            
            await FileIO.WriteBytesAsync(file, await httpContent.ReadAsByteArrayAsync());

            return file;
        }

        public async Task<string> GetDidacticsItemTextAsync(Content content)
        {
            string requestUri = string.Format(DIDACTICS_ATTACHMENT_URL, authResponse.UserID, content.ID);

            HttpContent httpContent = await SendRequestAsync(HttpMethod.Get, requestUri, null);
            DidacticsTextContentResponse response = JsonConvert.DeserializeObject<DidacticsTextContentResponse>(await httpContent.ReadAsStringAsync());

            return response.TextItem.Text;
        }

        public async Task<Uri> GetDidacticsItemLinkAsync(Content content)
        {
            string requestUri = string.Format(DIDACTICS_ATTACHMENT_URL, authResponse.UserID, content.ID);

            HttpContent httpContent = await SendRequestAsync(HttpMethod.Get, requestUri, null);
            DidacticsLinkContentResponse response = JsonConvert.DeserializeObject<DidacticsLinkContentResponse>(await httpContent.ReadAsStringAsync());

            return new Uri(response.LinkItem.Link);
        }

        private async Task<HttpContent> SendRequestAsync(HttpMethod method, string requestUri, object content)
        {
            using (HttpRequestMessage requestMessage = new HttpRequestMessage(method, requestUri))
            {
                requestMessage.Headers.Add("Z-Dev-Apikey", API_KEY);
                if (authResponse != null && !string.IsNullOrWhiteSpace(authResponse.Token))
                {
                    requestMessage.Headers.Add("Z-Auth-Token", authResponse.Token);
                }

                if (content != null)
                {
                    requestMessage.Content = new StringContent(JsonConvert.SerializeObject(content, Formatting.Indented), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

                if (response.ReasonPhrase.Equals("Unprocessable Entity"))
                {
                    throw new UnauthorizedUserException("UnauthorizedUserException".GetLocalized());
                }

                response.EnsureSuccessStatusCode();

                return response.Content;
            }
        }
    }

    // TODO: Implement the following call
    // @POST("students/{studentId}/notes/{type}/read/{layout_note}")
}
