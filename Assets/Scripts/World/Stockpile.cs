using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MoreLinq;

public class Stockpile
{
    private List<StockpileBlock> _stockpileBlocks;

    public Stockpile()
    {
        _stockpileBlocks = new List<StockpileBlock>();
    }

    public StockpileBlock GetClosestStockpileBlock(Vector3 position)
    {
        return _stockpileBlocks.MinBy(block => Vector3.Distance(block.Position, position));
    }

    public void AddStockpileBlock(StockpileBlock stockpileBlock)
    {
        _stockpileBlocks.Add(stockpileBlock);
    }
}
