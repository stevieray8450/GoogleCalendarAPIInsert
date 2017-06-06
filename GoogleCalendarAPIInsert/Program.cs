using System;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleCalendarAPIInsert
{
    class MainClass
    {
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly};
        //static string ApplicationName = "Google Calendar API Quickstart";

        public static void Main(string[] args)
        {
            UserCredential credential;

            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/calendar-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                //ApplicationName = "Google Calendar API Quickstart",
            });

			Event newEvent = new Event()
			{
				Summary = "Google I/O 2015",
				Location = "800 Howard St., San Francisco, CA 94103",
				Description = "A chance to hear more about Google's developer products.",
				Start = new EventDateTime()
				{
					DateTime = DateTime.Parse("2015-05-28T09:00:00-07:00"),
					TimeZone = "America/Los_Angeles",
				},
				End = new EventDateTime()
				{
					DateTime = DateTime.Parse("2015-05-28T17:00:00-07:00"),
					TimeZone = "America/Los_Angeles",
				},
				Recurrence = new String[] { "RRULE:FREQ=DAILY;COUNT=2" },
				Attendees = new EventAttendee[] {
					new EventAttendee() { Email = "lpage@example.com" },
					new EventAttendee() { Email = "sbrin@example.com" },
	                },
				Reminders = new Event.RemindersData()
				{
					UseDefault = false,
					Overrides = new EventReminder[] {
						new EventReminder() { Method = "email", Minutes = 24 * 60 },
						new EventReminder() { Method = "sms", Minutes = 10 },
					}
				}
			};

			String calendarId = "primary";
			EventsResource.InsertRequest request = service.Events.Insert(newEvent, calendarId);
			Event createdEvent = request.Execute();
			Console.WriteLine("Event created: {0}", createdEvent.HtmlLink);

        }
    }
}
