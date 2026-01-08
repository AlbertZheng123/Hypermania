using System.Collections;
using System.Collections.Generic;
using Design.Animation;
using UnityEngine;

namespace Game.Sim
{
    public class Physics<TData>
    {
        public struct Collision
        {
            public BoxEntry BoxA;
            public BoxEntry BoxB;
            public float OverlapX;
        }

        public struct BoxEntry
        {
            public int Owner;
            public Box Box;
            public TData Data;
        }

        // TODO: move this out into a utils file
        public struct Box
        {
            public Vector2 Pos;
            public Vector2 Size;

            public bool Overlaps(Box b, out float overlapX)
            {
                Vector2 ah = Size * 0.5f;
                Vector2 bh = b.Size * 0.5f;

                float aMinX = Pos.x - ah.x;
                float aMaxX = Pos.x + ah.x;
                float aMinY = Pos.y - ah.y;
                float aMaxY = Pos.y + ah.y;

                float bMinX = b.Pos.x - bh.x;
                float bMaxX = b.Pos.x + bh.x;
                float bMinY = b.Pos.y - bh.y;
                float bMaxY = b.Pos.y + bh.y;

                float ox = Mathf.Min(aMaxX, bMaxX) - Mathf.Max(aMinX, bMinX);
                if (ox <= 0f)
                {
                    overlapX = 0f;
                    return false;
                }

                float oy = Mathf.Min(aMaxY, bMaxY) - Mathf.Max(aMinY, bMinY);
                if (oy <= 0f)
                {
                    overlapX = 0f;
                    return false;
                }

                overlapX = ox;
                return true;
            }
        }

        private readonly Pool<BoxEntry> _boxPool;
        private readonly List<int> _boxInds;
        private readonly Dictionary<ulong, Collision> _collisions = new Dictionary<ulong, Collision>(64);

        public Physics(int maxHitboxes)
        {
            _boxPool = new Pool<BoxEntry>(maxHitboxes);
            _boxInds = new List<int>(maxHitboxes);
        }

        public int AddBox(int handle, Vector2 boxPos, Vector2 boxSize, in TData data)
        {
            int ind = _boxPool.Spawn(
                new BoxEntry
                {
                    Owner = handle,
                    Box = new Box { Pos = boxPos, Size = boxSize },
                    Data = data,
                }
            );
            _boxInds.Add(ind);
            return ind;
        }

        // TODO: optimize?
        public void GetCollisions(List<Collision> collisions)
        {
            int n = _boxInds.Count;
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    int boxAInd = _boxInds[i];
                    int boxBInd = _boxInds[j];
                    BoxEntry a = _boxPool[boxAInd];
                    BoxEntry b = _boxPool[boxBInd];

                    if (a.Owner == b.Owner)
                    {
                        continue;
                    }

                    if (a.Box.Overlaps(b.Box, out float overlapX))
                    {
                        collisions.Add(
                            new Collision
                            {
                                BoxA = a,
                                BoxB = b,
                                OverlapX = overlapX,
                            }
                        );
                    }
                }
            }
        }

        public void Clear()
        {
            _boxPool.Clear();
            _collisions.Clear();
            _boxInds.Clear();
        }
    }
}
