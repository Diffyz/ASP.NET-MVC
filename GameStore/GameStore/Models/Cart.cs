﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Models
{
    public class Cart
    {
        public class CartLine
        {
            public Game Game { get; set; }
            public int Quantity { get; set; }
        }

        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Game game,int quantity)
        {
            CartLine line = lineCollection
                .Where(p => p.Game.GameId == game.GameId)
                .FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Game = game,
                    Quantity = quantity

                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Game game)
        {
            lineCollection.RemoveAll(p => p.Game.GameId == game.GameId);
        }
        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(p => p.Game.Price * p.Quantity);
        }
        public void Clear()
        {
            lineCollection.Clear();
        }
        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }
}