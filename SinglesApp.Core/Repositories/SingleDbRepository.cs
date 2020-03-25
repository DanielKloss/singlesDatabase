using SinglesApp.Core.Models;
using SinglesApp.Core.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SinglesApp.Core.Repositories
{
    public class SingleDbRepository
    {
        SQLiteConnection _connection;
        internal string databaseLocation;

        public SingleDbRepository(string DatabaseLocation)
        {
            this.databaseLocation = DatabaseLocation;

            try
            {
                _connection = new SQLiteConnection(this.databaseLocation);
            }
            catch (SQLiteException)
            {
                throw;
            }
        }

        internal void UpdateSingle(SingleViewModel recordToUpdate)
        {
            int artistId = GetArtistId(recordToUpdate);
            Singles singleToChange = MapSingleData(recordToUpdate, artistId);
            UpdateGenres(recordToUpdate);
            _connection.Update(singleToChange);
        }

        private Singles MapSingleData(SingleViewModel recordToUpdate, int artistId)
        {
            Singles singleToChange = new Singles()
            {
                SingleId = recordToUpdate.singleId,
                ASideTitle = recordToUpdate.aSideTitle,
                BSideTitle = recordToUpdate.bSideTitle,
                SingleYear = recordToUpdate.singleYear,
                Dinked = recordToUpdate.dinked,
                JukeboxCardPrinted = recordToUpdate.jukeboxCardPrinted,
                PictureCover = recordToUpdate.pictureCover,
                SingleLabelPrinted = recordToUpdate.singleLabelPrinted,
                ArtistId = artistId
            };

            return singleToChange;
        }

        private void UpdateGenres(SingleViewModel recordToUpdate)
        {
            List<string> splitGenres = recordToUpdate.genreName?.Split(',').ToList() == null ? new List<string>() : recordToUpdate.genreName?.Split(',').ToList();

            foreach (string genre in splitGenres)
            {
                int genreId;
                string neatGenre = genre.Trim().ToLower();

                List<Genres> genres = _connection.Table<Genres>().Where(g => g.GenreName == neatGenre).ToList();

                if (genres.Count == 0)
                {
                    genreId = InsertGenre(neatGenre);
                }
                else
                {
                    genreId = genres.First().GenreId;
                }

                List<SingleGenres> singleGenres = _connection.Table<SingleGenres>().Where(sg => sg.SingleId == recordToUpdate.singleId && sg.GenreId == genreId).ToList();

                if (singleGenres.Count == 0)
                {
                    InsertSingleGenre(recordToUpdate.singleId, genreId);
                }
            }
        }

        private int GetArtistId(SingleViewModel recordToUpdate)
        {
            List<Artists> artists = _connection.Table<Artists>().Where(a => a.ArtistName == recordToUpdate.artistName).ToList();

            if (artists.Count == 0)
            {
                return InsertArtist(recordToUpdate.artistName);
            }
            else
            {
                return artists.First().ArtistId;
            }
        }

        internal IEnumerable<JukeboxSelections> GetSelections()
        {
            return _connection.Query<JukeboxSelections>("SELECT SelectionName, SelectionId FROM [JukeboxSelections]");
        }

        internal IEnumerable<SingleViewModel> GetSingleSelections(int singleId)
        {
            return _connection.Query<SingleViewModel>("SELECT singleId FROM [JukeboxSelectionSingles] WHERE SingleId = ?", singleId);
        }

        internal IEnumerable<SingleViewModel> GetAllSingles()
        {
            return _connection.Query<SingleViewModel>("SELECT [Singles].singleId, aSideTitle, bSideTitle, artistName, genreName, pictureCover, singleYear, dinked, jukeboxCardPrinted, singleLabelPrinted FROM [Singles] " +
                                                     "LEFT JOIN [Artists] ON Singles.ArtistId = Artists.ArtistId " +
                                                     "LEFT JOIN [SingleGenres] ON Singles.SingleId = SingleGenres.SingleId " +
                                                     "LEFT JOIN [Genres] ON SingleGenres.GenreId = Genres.GenreId ");
        }

        private int InsertSingleGenre(int singleId, int genreId)
        {
            _connection.Insert(new SingleGenres() { SingleId = singleId, GenreId = genreId });
            return _connection.ExecuteScalar<int>("SELECT LAST_INSERT_ROWID()");
        }

        internal int AddSingleToCollection(SingleViewModel recordToAdd)
        {
            int artistId = GetArtistId(recordToAdd);
            Singles singleToAdd = MapSingleData(recordToAdd, artistId);
            _connection.Insert(singleToAdd);
            return _connection.ExecuteScalar<int>("SELECT LAST_INSERT_ROWID()");
        }

        internal void RemoveSingleFromCollection(SingleViewModel singleToRemove)
        {
            int artistId = GetArtistId(singleToRemove);
            Singles recordToRemove = MapSingleData(singleToRemove, artistId);

            _connection.Delete(recordToRemove);
        }

        internal JukeboxSelections CreateNewSelection()
        {
            _connection.Insert(new JukeboxSelections() { selectionName = DateTime.Now.ToString() });
            int selectionId = _connection.ExecuteScalar<int>("SELECT LAST_INSERT_ROWID()");

            return _connection.Query<JukeboxSelections>("SELECT selectionId, selectionName FROM [JukeboxSelections] WHERE selectionId = ?", selectionId).FirstOrDefault();
        }

        internal void AddSingleToSelection(int singleId, int selectionId)
        {
            _connection.Insert(new JukeboxSelectionSingles() { SelectionId = selectionId, SingleId = singleId });
        }

        internal void RemoveSingleFromSelection(int singleId, int selectionId)
        {
            _connection.Delete(new JukeboxSelectionSingles() { SelectionId = selectionId, SingleId = singleId });
        }

        internal IEnumerable<SingleViewModel> GetSelectionSingles(int selectionId)
        {
            return _connection.Query<SingleViewModel>("SELECT [JukeboxSelectionSingles].singleId, aSideTitle, bSideTitle, artistName FROM [JukeboxSelectionSingles] " +
                                                              "INNER JOIN [Singles] ON JukeboxSelectionSingles.singleId = Singles.singleId " +
                                                              "INNER JOIN [Artists] ON Singles.singleId = Artists.artistId " +
                                                              "WHERE selectionId = ?", selectionId);
        }

        private int InsertGenre(string genre)
        {
            _connection.Insert(new Genres() { GenreName = genre });
            return _connection.ExecuteScalar<int>("SELECT LAST_INSERT_ROWID()");
        }

        public int InsertArtist(string ArtistName)
        {
            _connection.Insert(new Artists() { ArtistName = ArtistName });
            return _connection.ExecuteScalar<int>("SELECT LAST_INSERT_ROWID()");
        }
    }
}
