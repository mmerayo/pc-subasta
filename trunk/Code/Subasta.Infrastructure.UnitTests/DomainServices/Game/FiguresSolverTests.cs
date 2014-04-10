using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Subasta.Domain.Game;
using Subasta.DomainServices.Game;
using Subasta.DomainServices.Game.Algorithms.MCTS;

namespace Subasta.Infrastructure.UnitTests.DomainServices.Game
{
    [TestFixture]
    class FiguresSolverTests
    {
        private TestContext _context;

        [SetUp]
        public void OnSetUp()
        {
            _context = new TestContext();
        }

        [TestCaseSource("CanGetSay_TestCases")]
        public IFigure CanGetSay(ISaysStatus saysStatus)
        {
            _context.WithStatus(saysStatus);
            return _context.Sut.GetFigure(saysStatus);
        }

        public static IEnumerable CanGetSay_TestCases()
        {
            //marca cada una de las figuras

            //en caso de mas de una hace el menor incremento

            //si se pasa y no ha marcado marca las alternativas 2->3->8
			
			//caballos o reyes tienen prioridad
			
			//caballos o reyes no se marcan si no sale el companyero
			
			//no marca parejas peladas 
			
			//con los caballos o los reyes no marca parejas si los consigue marcar
			
			//con los caballos o los reyes marca las parejas si no los consigue marcar
			
			//si marca un as no marca noventa en el mismo marque
			
			//pareja sin marcar, marca unamas si hay margen sino cierra subasta
			
			//cuando el companyero esta pasado cierra subasta
			
			//cuando no tiene nada mas que marcar y tiene la mayor puntuacion del palo "pasa" si no viene pasada, sino sube una mas(en forma de "a X"), si hay margen de 2. 
			
			//cuando hay margen de 1, cierra subasta indicando "a x" e.g. "a diez"

            //TODO:
            return null;
        }

        private class TestContext
        {
            private readonly Fixture _fixture;
            private FiguresSolver _sut;
            public FiguresSolver Sut
            {
                get { return _sut ?? (_sut = _fixture.Create<FiguresSolver>()); }
            }

            public TestContext WithStatus(ISaysStatus saysStatus)
            {
                //TODO: PREPARE MOCKS

                return this;
            }
        }
    }
}
