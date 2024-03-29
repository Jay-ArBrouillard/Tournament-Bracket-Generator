﻿using System.Collections.Generic;
using TBG.Core.Interfaces;

namespace TBG.Business.Controllers
{
    public class DashboardController
    {

        /// <summary>
        /// Represents all existing tournaments.
        /// </summary>
        public List<ITournament> Tournaments { get; set; } = new List<ITournament>();

        /// <summary>
        /// Represents only the tournaments fully completed.
        /// </summary>
        public List<ITournament> TournamentsCompleted { get; set; } = new List<ITournament>();

        /// <summary>
        /// Represents only the tournaments not completed.
        /// </summary>
        public List<ITournament> TournamentsPending { get; set; } = new List<ITournament>();
    }
}
