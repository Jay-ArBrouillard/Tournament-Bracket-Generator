using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TBG.Core.Interfaces;

namespace TBG.Business.Helpers
{
    public static class NotificationHelper
    {
        public static void NotifyParticipants(IMatchup matchup, ITournament tournament)
        {
            List<IMatchupEntry> matchupEntries = matchup.MatchupEntries;
            string firstTeamName = tournament.Teams.Find(x => x.TeamId == matchupEntries[0].TheTeam.TeamId).TeamName;
            string secondTeamName = tournament.Teams.Find(x => x.TeamId == matchupEntries[1].TheTeam.TeamId).TeamName;
            var finals = tournament.Rounds.Max(x => x.RoundNum) == tournament.ActiveRound; ;
            Email(matchupEntries[0], firstTeamName, secondTeamName, tournament.TournamentName, tournament.ActiveRound, finals);
            Email(matchupEntries[1], secondTeamName, firstTeamName, tournament.TournamentName, tournament.ActiveRound, finals);
        }
        private static void Email(IMatchupEntry matchupEntry, string competitor, string opponent, string tournamentName, int roundNum, bool finals)
        {
            foreach (var members in matchupEntry.TheTeam.Members)
            {
                string matchupString = "Your next matchup is against: " + opponent;

                if (finals)
                {
                    matchupString = "Welcome to the finals against: " + opponent;
                }

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                sendEmail(members.Email,
                    $"{members.FirstName} {members.LastName}",
                    $"{tournamentName} Tournament: Round {roundNum} ready to start",
                    $"Hello {competitor}!" +
                    $"\nRound {roundNum} is ready to start.\n" +
                    matchupString +
                    ".\nPlease report to the scorers table for location information.");
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }
        private static async Task sendEmail(string toAddr, string toName, string subject, string body)
        {
            AppSettingsReader settingsReader = new AppSettingsReader();

            var fromAddr = (string)settingsReader.GetValue("EmailFrom", typeof(String));
            var emailPassword = (string)settingsReader.GetValue("EmailPassword", typeof(String));
            var displayname = (string)settingsReader.GetValue("EmailDisplayName", typeof(String));

            var fromAddress = new MailAddress(fromAddr, displayname);
            var toAddress = new MailAddress(toAddr, toName);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, emailPassword),
                Timeout = 20000
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                await smtp.SendMailAsync(message);
            }
        }
    }
}