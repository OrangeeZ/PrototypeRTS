using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using UnityEngine;

public class Stockpile
{
    private readonly List<StockpileBlock> _stockpileBlocks;

    public Stockpile()
    {
        _stockpileBlocks = new List<StockpileBlock>();
    }

    public List<StockpileBlock> GetBlocks()
    {
        return _stockpileBlocks;
    }

    public StockpileBlock GetClosestStockpileBlock(Vector3 position, ResourceInfo resourceToStore)
    {
        var filterByResource = _stockpileBlocks.Where(block => block.CanStore(resourceToStore, 1)).ToArray();
        if (!filterByResource.Any())
            return null;
        return filterByResource.MinBy(block => Vector3.Distance(block.Position, position));
    }

    public void AddStockpileBlock(StockpileBlock stockpileBlock)
    {
        _stockpileBlocks.Add(stockpileBlock);
    }

    public bool HasResource(ResourceInfo resourceInfo)
    {
        return _stockpileBlocks.Any(_ => _.HasResource(resourceInfo.Id));
    }

    //Stub method until later
    public int GetTotalResourceAmount(string resourceId)
    {
        return _stockpileBlocks.Sum(_ => _[resourceId]);
    }

    //Yes, this is a miraculous stub as well
    //This is fast
    public void ChangeTotalResourceAmount(string resourceId, int amount)
    {
        while (GetTotalResourceAmount(resourceId) > 0)
        {
            var firstStockpileWithResource = _stockpileBlocks.First(_ => _.HasResource(resourceId));
            firstStockpileWithResource.ChangeResource(resourceId, -1);
        }
    }

    public StockpileBlock GetClosestStockpileWithResource(Vector3 position, ResourceInfo resourceInfo)
    {
        if (resourceInfo == null)
        {
            return null;
        }
        
        var blocksWithResource = _stockpileBlocks.Where(_ => _.HasResource(resourceInfo.Id));
        var stockpileBlocks = blocksWithResource as StockpileBlock[] ?? blocksWithResource.ToArray();
        if (stockpileBlocks.Any())
        {
            var closestBlock = stockpileBlocks.MinBy(_ => Vector3.Distance(_.Position, position));

            return closestBlock;            
        }

        return null;
    }
}