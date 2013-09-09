﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SmartQuant.Data;
using SmartQuant.FIX;
using SmartQuant.Instruments;
using SmartQuant.Providers;

namespace QuantBox.OQ.TongShi
{
    public partial class APIProvider : IMarketDataProvider, ISimulationMarketDataProvider
    {
        private void OnNewBar(object sender, BarEventArgs args)
        {
            if (NewBar != null)
            {
                Bar bar = args.Bar;

                if (null != MarketDataFilter)
                {
                    Bar b = MarketDataFilter.FilterBar(bar, args.Instrument.Symbol);
                    if (null != b)
                    {
                        NewBar(this, new BarEventArgs(b, args.Instrument, this));
                    }
                }
                else
                {
                    NewBar(this, new BarEventArgs(bar, args.Instrument, this));
                }
            }
        }

        private void OnNewBarOpen(object sender, BarEventArgs args)
        {
            if (NewBarOpen != null)
            {
                Bar bar = args.Bar;
                
                if (null != MarketDataFilter)
                {
                    Bar b = MarketDataFilter.FilterBarOpen(bar, args.Instrument.Symbol);
                    if (null != b)
                    {
                        NewBarOpen(this, new BarEventArgs(b, args.Instrument, this));
                    }
                }
                else
                {
                    NewBarOpen(this, new BarEventArgs(bar, args.Instrument, this));
                }
            }
        }

        private void EmitNewQuoteEvent(IFIXInstrument instrument, Quote quote)
        {
            if (this.MarketDataFilter != null)
            {
                quote = this.MarketDataFilter.FilterQuote(quote, instrument.Symbol);
            }

            if (quote != null)
            {
                if (NewQuote != null)
                {
                    NewQuote(this, new QuoteEventArgs(quote, instrument, this));
                }
                if (factory != null)
                {
                    factory.OnNewQuote(instrument, quote);
                }
            }
        }

        private void EmitNewTradeEvent(IFIXInstrument instrument, Trade trade)
        {
            if (this.MarketDataFilter != null)
            {
                trade = this.MarketDataFilter.FilterTrade(trade, instrument.Symbol);
            }

            if (trade != null)
            {
                if (NewTrade != null)
                {
                    NewTrade(this, new TradeEventArgs(trade, instrument, this));
                }
                if (factory != null)
                {
                    factory.OnNewTrade(instrument, trade);
                }
            }
        }

        #region OpenQuant3接口的新方法
        public IMarketDataFilter MarketDataFilter { get; set; }

        public void EmitQuote(IFIXInstrument instrument, Quote quote)
        {
            EmitNewQuoteEvent(instrument, quote);
        }

        public void EmitTrade(IFIXInstrument instrument, Trade trade)
        {
            EmitNewTradeEvent(instrument, trade);
        }
        #endregion
    }
}
