using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Web.Controllers;
using Web.Data;
using Web.Helpers;
using Web.Models;
using WebAPIXUnitTestProject.Helpers;
using Xunit;

namespace WebAPIXUnitTestProject
{
    public class MoviesControllerUnitTest
    {
        #region SetUp

        private readonly MoviesController _controller;
        private readonly IQueryable<Movie> _testSongs;

        public MoviesControllerUnitTest()
        {
            _testSongs = new List<Movie>
            {
                new Movie
                {
                    Id = 1,
                    Year = 2001,
                    Genre = "Comedy",
                    Producer = "Steven Spielberg",
                    Title = "King Kong",
                    About = "Monkey"
                },
                new Movie
                {
                    Id = 2,
                    Year = 2002,
                    Genre = "Drama",
                    Producer = "James Cameron",
                    Title = "Titanic",
                    About = "Boat"
                },
                new Movie
                {
                    Id = 3,
                    Year = 2003,
                    Genre = "Crime",
                    Producer = "Steven Spielberg",
                    Title = "Shindler's List",
                    About = "A list"
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Movie>>();
            mockSet.As<IAsyncEnumerable<Movie>>()
                .Setup(d => d.GetEnumerator())
                .Returns(new TestDbAsyncEnumerator<Movie>(_testSongs.GetEnumerator()));

            mockSet.As<IQueryable<Movie>>().Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<Movie>(_testSongs.Provider));
            mockSet.As<IQueryable<Movie>>().Setup(m => m.Expression).Returns(_testSongs.Expression);
            mockSet.As<IQueryable<Movie>>().Setup(m => m.ElementType).Returns(_testSongs.ElementType);
            mockSet.As<IQueryable<Movie>>().Setup(m => m.GetEnumerator()).Returns(_testSongs.GetEnumerator());

            var dbContext = new Mock<DataContext>();
            dbContext.Setup(dc => dc.Movies).Returns(mockSet.Object);
            dbContext.Setup(dc => dc.Set<Movie>()).Returns(mockSet.Object);

            _controller = new MoviesController(dbContext.Object);
        }

        #endregion

        [Fact]
        public void CanGetAllMovies()
        {
            var result = _controller.GetMovie(new MovieFilter());
            Assert.Equal(result, _testSongs);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task CanGetMovieByIdAsync(int id)
        {
            var result = await _controller.GetMovie(id);

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var movie = okResult.Value.Should().BeAssignableTo<Movie>().Subject;

            movie.Id.Should().Be(id);
        }
        
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(10000)]
        public async Task CannotGetMovie(int id)
        {
            var result = await _controller.GetMovie(id);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void CanFilterMoviesByYear()
        {
            var filter = new MovieFilter
            {
                Year = 2002
            };
            var result = _controller.GetMovie(filter);

            var movies = result.Should().BeAssignableTo<IEnumerable<Movie>>().Subject;

            movies.Should().OnlyContain(movie => movie.Year == filter.Year);
        }

        [Fact]
        public void CanFilterMoviesByGenre()
        {
            var filter = new MovieFilter
            {
                Genre = "Comedy"
            };
            var result = _controller.GetMovie(filter);

            var movies = result.Should().BeAssignableTo<IEnumerable<Movie>>().Subject;

            movies.Should().OnlyContain(movie => movie.Genre == filter.Genre);
        }

        [Fact]
        public void CanFilterMoviesByProducer()
        {
            var filter = new MovieFilter
            {
                Producer = "Steven Spielberg",
            };
            var result = _controller.GetMovie(filter);

            var movies = result.Should().BeAssignableTo<IEnumerable<Movie>>().Subject;

            movies.Should().OnlyContain(movie => movie.Producer == filter.Producer);
        }

        [Fact]
        public void CanPaginateResults()
        {
            var filter = new MovieFilter
            {
                Page = 2,
                Size = 2
            };
            var result = _controller.GetMovie(filter);

            var movies = result.Should().BeAssignableTo<IEnumerable<Movie>>().Subject;

            movies.First().Id.Should().Be(3);
        }

        [Fact]
        public async Task CanPostMovie()
        {
            var movie = new Movie
            {
                Id = 4,
                Year = 2004,
                Genre = "Adventure",
                Producer = "Steven Spielberg",
                Title = "Avatar",
                About = "Blue people"
            };

            var result = await _controller.PostMovie(movie);

            var okResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            var resultSong = okResult.Value.Should().BeAssignableTo<Movie>().Subject;
            resultSong.Id.Should().Be(movie.Id);
        }
        
        [Fact]
        public async Task CanDeleteMovie()
        {
            var result = await _controller.DeleteMovie(2);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task CannotFindMovieToDelete()
        {
            var result = await _controller.DeleteMovie(20);

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}