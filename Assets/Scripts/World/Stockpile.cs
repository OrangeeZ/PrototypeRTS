using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using NUnit.Framework;
using UnityEngine;

public class Stockpile
{
    private List<StockpileBlock> _stockpileBlocks;

    public Stockpile()
    {
        _stockpileBlocks = new List<StockpileBlock>();
    }

    public List<StockpileBlock> GetBlocks()
    {
        return _stockpileBlocks;
    }

    public StockpileBlock GetClosestStockpileBlock(Vector3 position)
    {
        return _stockpileBlocks.MinBy(block => Vector3.Distance(block.Position, position));
    }

    public void AddStockpileBlock(StockpileBlock stockpileBlock)
    {
        _stockpileBlocks.Add(stockpileBlock);
    }

    public bool HasResource(ResourceInfo resourceInfo)
    {
        return _stockpileBlocks.Any(_ => _.HasResource(resourceInfo.Id));
    }

    public StockpileBlock GetClosestStockpileWithResource(Vector3 position, ResourceInfo resourceInfo)
    {
        if (resourceInfo == null)
        {
            return null;
        }
        
        var blocksWithResource = _stockpileBlocks.Where(_ => _.HasResource(resourceInfo.Id));

        if (blocksWithResource.Any())
        {
            var closestBlock = blocksWithResource.MinBy(_ => Vector3.Distance(_.Position, position));

            return closestBlock;            
        }

        return null;
    }
}