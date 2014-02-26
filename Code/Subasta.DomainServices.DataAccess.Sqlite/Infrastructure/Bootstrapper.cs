﻿using System;
using System.Collections.Generic;
using AutoMapper;
using Subasta.ApplicationServices;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices.DataAccess.Sqlite.Models;

namespace Subasta.DomainServices.DataAccess.Sqlite.Infrastructure
{
    class Bootstrap:IBootstrap
    {
        private static bool _executed = false;

        public void Execute()
        {
            if (!_executed)
                lock (this)
                    if (!_executed)
                    {
                        RegisterCardsMappings();
                        RegisterHandsMappings();
                        RegisterExplorationMappings();
#if DEBUG
                        Mapper.AssertConfigurationIsValid();
#endif
                        _executed = true;
                    }
        }

        private static void RegisterExplorationMappings()
        {
            Mapper.CreateMap<NodeResult, ExplorationInfo>().ConvertUsing<ExplorationConverter>();
        }

        private static void RegisterHandsMappings()
        {
            Mapper.CreateMap<IHand, HandInfo>().ConvertUsing<HandConverter>();
        }

        private static void RegisterCardsMappings()
        {
            Mapper.CreateMap<ICard, CardInfo>().ConvertUsing<CardConverter>();
        }

        private abstract class CustomConverter<TSource,TDestination>:ITypeConverter<TSource,TDestination> where TSource : class
            where TDestination:class
        {
            public TDestination Convert(ResolutionContext context)
            {
                var sourceValue = (TSource)context.SourceValue;
                if (sourceValue == null)
                    return null;
                return DoConvert(sourceValue);
            }

            protected abstract TDestination DoConvert(TSource sourceValue);
        }

        private class CardConverter : CustomConverter<ICard, CardInfo>
        {
            protected override CardInfo DoConvert(ICard sourceValue)
            {
                return new CardInfo
                {
                    Number = sourceValue.Number,
                    Suit = sourceValue.Suit.Name
                };
            }
        }
        private class HandConverter : CustomConverter<IHand, HandInfo>
        {
            protected override HandInfo DoConvert(IHand sourceValue)
            {
                var cards = new List<CardInfo>
                                        {
                                            Mapper.Map<CardInfo>(sourceValue.PlayerCard(1)),
                                           Mapper.Map<CardInfo>( sourceValue.PlayerCard(2)),
                                           Mapper.Map<CardInfo>( sourceValue.PlayerCard(3)),
                                           Mapper.Map<CardInfo>( sourceValue.PlayerCard(4))
                                        };

                return new HandInfo
                           {
                               Cards = Mapper.Map<CardInfo[]>(cards),
                               Declaration =
                                   sourceValue.Declaration.HasValue ? sourceValue.Declaration.Value.ToString() : null,
                               FirstPlayer = sourceValue.FirstPlayer,
                               Sequence=sourceValue.Sequence
                           };
            }
        }

        private class ExplorationConverter:CustomConverter<NodeResult,ExplorationInfo>
        {
            protected override ExplorationInfo DoConvert(NodeResult sourceValue)
            {
                return new ExplorationInfo
                           {
                               Hands = Mapper.Map<HandInfo[]>(sourceValue.Hands),
                               PointsTeam1 = sourceValue.Points1And3,
                               PointsTeam2 = sourceValue.Points2And4,
                               Trump= sourceValue.Trump.Name
                           };
            }
        }

        
    }

  
}
