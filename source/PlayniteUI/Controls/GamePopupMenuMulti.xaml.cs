﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Playnite;
using Playnite.Database;
using Playnite.Models;

namespace PlayniteUI.Controls
{
    /// <summary>
    /// Interaction logic for GamePopupMenuMulti.xaml
    /// </summary>
    public partial class GamePopupMenuMulti : Popup, INotifyPropertyChanged
    {
        private bool showRemoveButton = false;
        public bool ShowRemoveButton
        {
            get
            {
                return showRemoveButton;
            }

            set
            {
                showRemoveButton = value;
                OnPropertyChanged("ShowRemoveButton");
            }
        }

        private bool showHideUnButton = false;
        public bool ShowUnHideButton
        {
            get
            {
                return showHideUnButton;
            }

            set
            {
                showHideUnButton = value;
                OnPropertyChanged("ShowUnHideButton");
            }
        }

        private bool showHideButton = false;
        public bool ShowHideButton
        {
            get
            {
                return showHideButton;
            }

            set
            {
                showHideButton = value;
                OnPropertyChanged("ShowHideButton");
            }
        }

        private bool showAddFavorite = false;
        public bool ShowAddFavorite
        {
            get
            {
                return showAddFavorite;
            }

            set
            {
                showAddFavorite = value;
                OnPropertyChanged("ShowAddFavorite");
            }
        }

        private bool showRemoveFavorite = false;
        public bool ShowRemoveFavorite
        {
            get
            {
                return showRemoveFavorite;
            }

            set
            {
                showRemoveFavorite = value;
                OnPropertyChanged("ShowRemoveFavorite");
            }
        }

        public GamePopupMenuMulti()
        {
            InitializeComponent();
            DataContextChanged += GamePopupMenuMulti_DataContextChanged;
        }

        private void GamePopupMenuMulti_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var games = (List<IGame>)e.NewValue;
            var allHidden = !games.Any(a => a.Hidden == false);
            var allUnHidden = !games.Any(a => a.Hidden == true);
            var onlyCustomGames = !games.Any(a => a.Provider != Provider.Custom);

            ShowRemoveButton = onlyCustomGames;
            ShowUnHideButton = allHidden;
            ShowHideButton = allUnHidden;
            ShowAddFavorite = games.Any(a => a.Favorite == false);
            ShowRemoveFavorite = games.Any(a => a.Favorite == true);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Hide_Click(object sender, RoutedEventArgs e)
        {
            foreach (var game in (List<IGame>)DataContext)
            {
                game.Hidden = true;
                GameDatabase.Instance.UpdateGameInDatabase(game);
            }

            IsOpen = false;
        }

        private void UnHide_Click(object sender, RoutedEventArgs e)
        {
            foreach (var game in (List<IGame>)DataContext)
            {
                game.Hidden = false;
                GameDatabase.Instance.UpdateGameInDatabase(game);
            }

            IsOpen = false;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var games = (List<IGame>)DataContext;
            GamesEditor.Instance.EditGames(games);
            IsOpen = false;
        }

        private void Categories_Click(object sender, RoutedEventArgs e)
        {
            var games = (List<IGame>)DataContext;
            GamesEditor.Instance.SetGamesCategories(games);
            IsOpen = false;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var games = (List<IGame>)DataContext;
            if (PlayniteMessageBox.Show(
                string.Format("Are you sure you want to remove {0} games?", games.Count),
                "Remove games?",
                MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                return;
            }

            foreach (var game in games)
            {
                GameDatabase.Instance.DeleteGame(game);
            }
        }

        private void AddFavorite_Click(object sender, RoutedEventArgs e)
        {
            foreach (var game in (List<IGame>)DataContext)
            {
                game.Favorite = true;
                GameDatabase.Instance.UpdateGameInDatabase(game);
            }

            IsOpen = false;
        }

        private void RemoveFavorite_Click(object sender, RoutedEventArgs e)
        {
            foreach (var game in (List<IGame>)DataContext)
            {
                game.Favorite = false;
                GameDatabase.Instance.UpdateGameInDatabase(game);
            }

            IsOpen = false;
        }
    }
}
