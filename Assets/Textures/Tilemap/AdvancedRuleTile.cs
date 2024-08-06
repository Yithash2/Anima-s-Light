using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

[CreateAssetMenu(menuName = "Advanced Rule Tile")]
public class AdvancedRuleTile : RuleTile<AdvancedRuleTile.Neighbor> {
    public bool AlwaysConnect;
    public TileBase[] TilesToConnect;
    public bool CheckSelf;

    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int Any = 3;
        public const int Specified = 4;
        public const int Nothing = 5;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case Neighbor.This: return CheckThis(tile);
            case Neighbor.NotThis: return CheckNotThis(tile);
            case Neighbor.Any: return CheckAny(tile);
            case Neighbor.Specified: return CheckSpecified(tile);
            case Neighbor.Nothing: return CheckNothing(tile);
        }
        return base.RuleMatch(neighbor, tile);
    }

    bool CheckThis(TileBase tile) {
        if(!AlwaysConnect)return tile == this;
        else return TilesToConnect.Contains(tile) || tile == this;
    }

    bool CheckNotThis(TileBase tile) {
        return tile != this;
    }

    bool CheckAny(TileBase tile) {
        if(CheckSelf) return tile != null;
        else return tile != null && tile != this;
    }

    bool CheckSpecified(TileBase tile){
        return TilesToConnect.Contains(tile);
    }

    bool CheckNothing(TileBase tile){
        return tile == null;
    }
}