using System.Numerics;

namespace Solution {
    using System;

    public class BattleshipField {
        public static bool ValidateBattlefield(int[,] field) {
            int height = field.GetLength(0);
            int width = field.GetLength(1);
            HashSet<(int, int)>?[,] unions = new HashSet<(int, int)>[height, width];
            UnionShips(field, height, width, unions);

            return ValidateUnions(unions);
        }

        private static void UnionShips(int[,] field, int height, int width, HashSet<(int, int)>?[,] unions) {
            for (int i = 0; i < height; i++)
            for (int j = 0; j < width; j++)
                if (field[i, j] != 0)
                    Union(i, j, unions);
        }

        private static bool ValidateUnions(HashSet<(int, int)>?[,] unions) {
            HashSet<HashSet<(int, int)>> allUnions = new HashSet<HashSet<(int, int)>>();
            foreach (HashSet<(int, int)> union in unions)
                if (union != null)
                    allUnions.Add(union);

            return ValidateCounts(allUnions) && ValidateShapes(allUnions);
        }

        private static bool ValidateShapes(HashSet<HashSet<(int, int)>> allUnions) {
            foreach (HashSet<(int, int)> union in allUnions) {
                int minI = union.Min(pos => pos.Item1);
                int maxI = union.Max(pos => pos.Item1);
                int minJ = union.Min(pos => pos.Item2);
                int maxJ = union.Max(pos => pos.Item2);

                if (maxI - minI + 1 != union.Count && maxJ - minJ + 1 != union.Count)
                    return false;

                for (int i = minI; i <= maxI; i++)
                for (int j = minJ; j <= maxJ; j++)
                    if (!union.Contains((i, j)))
                        return false;
            }

            return true;
        }

        private static bool ValidateCounts(HashSet<HashSet<(int, int)>> allUnions) {
            Dictionary<int, int> shipCounts = new Dictionary<int, int>();
            foreach (HashSet<(int, int)> union in allUnions)
                if (!shipCounts.TryAdd(union.Count, 1))
                    shipCounts[union.Count]++;

            return shipCounts
                .OrderBy(kv => kv.Key)
                .Select(kv => (kv.Key, kv.Value))
                .SequenceEqual(new[] { (1, 4), (2, 3), (3, 2), (4, 1) });
        }

        private static void Union(int i, int j, HashSet<(int, int)>?[,] unions) {
            foreach (var adjacent in GetAdjacent(i, j)) {
                HashSet<(int, int)>? currentUnion = unions[i, j];
                HashSet<(int, int)>? adjacentUnion = unions[adjacent.Item1, adjacent.Item2];
                if (adjacentUnion != null) {
                    if (currentUnion == null) {
                        adjacentUnion.Add((i, j));
                        unions[i, j] = adjacentUnion;
                    }
                    else
                        CombineUnions(currentUnion, adjacentUnion, unions);
                }
            }

            if (unions[i, j] == null) {
                HashSet<(int, int)> newUnion = new HashSet<(int, int)>() { (i, j) };
                unions[i, j] = newUnion;
            }
        }

        private static void CombineUnions(HashSet<(int, int)> currentUnion, HashSet<(int, int)> adjacentUnion,
                                          HashSet<(int, int)>?[,] unions) {
            foreach (var pos in adjacentUnion) {
                currentUnion.Add(pos);
                unions[pos.Item1, pos.Item2] = currentUnion;
            }
        }

        private static IEnumerable<(int, int)> GetAdjacent(int i, int j) =>
            Directions
                .Select(direction => (i + direction.Item1, j + direction.Item2))
                .Where(pos => IsValid(pos.Item1, pos.Item2, 10, 10));

        private static readonly (int, int)[] Directions = new (int, int)[] {
            (-1, -1), (-1, 0), (-1, 1),
            (0, -1), (0, 1),
            (1, -1), (1, 0), (1, 1)
        };

        private static bool IsValid(int i, int j, int height, int width) =>
            i >= 0 && i < height && j >= 0 && j < width;
    }
}