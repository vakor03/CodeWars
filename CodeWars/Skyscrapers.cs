namespace CodeWars;

public class Skyscrapers {
    public static int[][] SolvePuzzle(int[] clues)
    {
        // Start your coding here...
        return null;
    }
}

public class BacktrackingAlgorithm {
    public int[][] SolvePuzzle(Board board) {
        if (IsSolved(board))
            return board.field;

        var nextCell = GetNextCell(board);
        var candidates = GetCandidates(board, nextCell);
        
        foreach (int candidate in candidates) {
            if (IsPromising(board, nextCell,  candidate)) {
                board.field[nextCell.x][nextCell.y] = candidate;
                var solve = SolvePuzzle(board);
                if (solve != null)
                    return solve;
                board.field[nextCell.x][nextCell.y] = 0;
            }
        }

        return null;
    }

    private bool IsPromising(Board board, Vector2Int nextCell, int candidate) {
        throw new NotImplementedException();
    }

    private bool IsSolved(Board board) =>
        board.IsSolved();

    private Vector2Int GetNextCell(Board board) {
        return new Vector2Int();
    }

    private IEnumerable<int> GetCandidates(Board board, Vector2Int cell) {
        for (int i = 0; i < 6; i++)
            yield return i;
    }
}

public struct Vector2Int {
    public int x;
    public int y;
}

public struct SkyscrapersField { }

public class Board {
    public int[][] field;
    public bool IsSolved() { }
}