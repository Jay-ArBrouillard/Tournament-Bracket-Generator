using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TBG.Core.Interfaces;
using TBG.Driver;
using TBG.UI.Models;

namespace TBG.UI
{
    public partial class ResultWindow : Window
    {
        private ITournament tournament;
        private ITournamentController tournamentController;
        private IProvider source;
        private Boolean initalize = true;

        public ResultWindow(ITournament tournament)
        {
            InitializeComponent();
            this.tournament = tournament;
            tournamentController = ApplicationController.getTournamentController();
            source = ApplicationController.getProvider();
            foreach (var round in tournament.Rounds)
            {
                roundSelectComboBox.Items.Add("Round " + round.RoundNum);
            }

            tournament = tournamentController.reSeedTournament(tournament);
            tournament.TournamentEntries = tournament.TournamentEntries.OrderByDescending(x => x.Seed).ToList();

            populateDataGrid(tournament.Rounds[0].Matchups);
            var placingColumn = resultDataGrid.Columns[2];
            resultDataGrid.Items.SortDescriptions.Add(new SortDescription(placingColumn.SortMemberPath, ListSortDirection.Ascending));
            initalize = false;  //Otherwise RoundSelectComboBox_SelectionChanged would be called twice
        }

        private void populateDataGrid(List<IRound> rounds)
        {
            foreach (var round in rounds)
            {
                populateDataGrid(round.Matchups);
            }
        }

        private void populateDataGrid(List<IMatchup> matchups)
        {
            List<IPrize> allPrizes = source.getAllPrizes();
            foreach (IPrize prize in allPrizes)
            {
                decimal convertPercent = prize.PrizePercent > 1 ? prize.PrizePercent / 100 : prize.PrizePercent;
                prize.PrizeAmount = Math.Round((decimal)tournament.TotalPrizePool * convertPercent, 2);
            }

            List<IResultDataRow> resultDataRows = tournamentController.populateResultsGrid(tournament, matchups, allPrizes);
            resultDataRows.ForEach(x => resultDataGrid.Items.Add(x));
        }

        private void RoundSelectComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            resultDataGrid.Items.Clear();
            int selectedRound = roundSelectComboBox.SelectedIndex;
            if (selectedRound == -1 || initalize == true)
            {
                return;
            }
            else if (selectedRound == tournament.Rounds.Count)
            {
                populateDataGrid(tournament.Rounds);
            }
            else
            {
                populateDataGrid(tournament.Rounds[selectedRound].Matchups);
            }
            var placingColumn = resultDataGrid.Columns[2];
            resultDataGrid.Items.SortDescriptions.Add(new SortDescription(placingColumn.SortMemberPath, ListSortDirection.Ascending));
        }
    }
}
