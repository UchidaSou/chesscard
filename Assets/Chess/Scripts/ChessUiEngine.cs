using System.Collections.Generic;
using UnityEngine;

public class ChessUiEngine : MonoBehaviour {
	enum Piece {King=0, Queen=1, Rook=2, Knight=3, Bishop=4, Pawn=5};
	private Piece[] setup = new Piece[] {Piece.Rook, Piece.Knight, Piece.Bishop, Piece.Queen, Piece.King, Piece.Bishop, Piece.Knight, Piece.Rook};
	private Piece[] demo = new Piece[]{Piece.Rook,Piece.Bishop,Piece.King,Piece.Bishop,Piece.Rook};
	public BoxCollider bounds;
	public List<Transform> whitePiecePrefabs;
	public List<Transform> blackPiecePrefabs;
	public GameObject board;
	BoardState boardState;

	public int RaycastCell(Ray ray) {
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 100)) {
			Vector3 point = hit.point + new Vector3 (-16, 0, 16);
			int i = (int)-point.x / 4;
			int j = (int)point.z / 4;
			return i * 8 + j;
		}
		return -1;
	}

	public void SetupPieces() {
		boardState = board.gameObject.GetComponent<BoardState>();
		for (int i = 0; i < 8; i++) {
			Transform piece = GameObject.Instantiate (whitePiecePrefabs [(int)setup [i]]);
			Vector3 worldPoint = ToWorldPoint (i);
			piece.position = new Vector3(worldPoint.x, piece.position.y, worldPoint.z);	
			Chess chess = piece.GetComponent<Chess>();
			chess.setFirstVector(piece.position);
			chess.setBeforeVector(piece.position);
			//State state = piece.GetComponent<State>();
			//state.setSetUp((int)setup[i]);
			//state.setColor("white");
			chess.setSetUp((int)setup[i]);
			//chess.setColor("white");
			boardState.chessBoardArray[0,i] = piece.gameObject;
			chess.boardState = boardState;
			chess.setMaxI(8);
			chess.setMaxJ(8);
		}
		for (int i = 0; i < 8; i++) {
			Transform piece = GameObject.Instantiate (blackPiecePrefabs [(int)setup [i]]);
			Vector3 worldPoint = ToWorldPoint (i+56);
			piece.position = new Vector3(worldPoint.x, piece.position.y, worldPoint.z);	
			Chess chess = piece.GetComponent<Chess>();
			chess.setFirstVector(piece.position);
			chess.setBeforeVector(piece.position);
			/*State state = piece.GetComponent<State>();
			state.setSetUp((int)setup[i]);
			state.setColor("black");
			*/
			chess.setSetUp((int)setup[i]);
			//chess.setColor("black");
			boardState.chessBoardArray[7,i] = piece.gameObject;
			chess.boardState = boardState;
			chess.setMaxI(8);
			chess.setMaxJ(8);
		}
		for (int i = 0; i < 8; i++) {
			Transform piece = GameObject.Instantiate (whitePiecePrefabs [(int)Piece.Pawn]);
			Vector3 worldPoint = ToWorldPoint (i+8);
			piece.position = new Vector3(worldPoint.x, piece.position.y, worldPoint.z);
			Chess chess = piece.GetComponent<Pawn>();
			chess.setFirstVector(piece.position);
			chess.setBeforeVector(piece.position);
			/*State state = piece.GetComponent<State>();
			state.setSetUp((int)Piece.Pawn);
			state.setColor("white");
			*/
			chess.setSetUp((int)Piece.Pawn);
			//chess.setColor("white");
			boardState.chessBoardArray[1,i] = piece.gameObject;
			chess.boardState = boardState;
			chess.setMaxI(8);
			chess.setMaxJ(8);
		}
		
		for (int i = 0; i < 8; i++) {
			Transform piece = GameObject.Instantiate (blackPiecePrefabs [(int)Piece.Pawn]);
			Vector3 worldPoint = ToWorldPoint (i+48);
			piece.position = new Vector3(worldPoint.x, piece.position.y, worldPoint.z);	
			Chess chess = piece.GetComponent<Pawn>();
			chess.setFirstVector(piece.position);
			chess.setBeforeVector(piece.position);
			/*State state = piece.GetComponent<State>();
			state.setSetUp((int)Piece.Pawn);
			state.setColor("black");
			*/
			chess.setSetUp((int)Piece.Pawn);
			//chess.setColor("black");
			boardState.chessBoardArray[6,i] = piece.gameObject;
			chess.boardState = boardState;
			chess.setMaxI(8);
			chess.setMaxJ(8);
		}
	}

	public void SetUpDemo(){
		boardState = board.gameObject.GetComponent<BoardState>();
		for (int i = 0; i < 5; i++) {
			Transform piece = GameObject.Instantiate (whitePiecePrefabs [(int)demo [i]]);
			Vector3 worldPoint = ToWorldPoint (i);
			piece.position = new Vector3(worldPoint.x, piece.position.y, worldPoint.z);	
			Chess chess = piece.GetComponent<Chess>();
			chess.setFirstVector(piece.position);
			chess.setBeforeVector(piece.position);
			/*State state = piece.GetComponent<State>();
			state.setSetUp((int)demo[i]);
			state.setColor("white");
			*/
			chess.setSetUp((int)demo[i]);
			//chess.setColor("white");
			boardState.chessBoardArray[0,i] = piece.gameObject;
			chess.boardState = boardState;
			chess.setMaxI(6);
			chess.setMaxJ(5);
		}
		for (int i = 0; i < 5; i++) {
			Transform piece = GameObject.Instantiate (blackPiecePrefabs [(int)demo [i]]);
			Vector3 worldPoint = ToWorldPoint (i+8*5);
			piece.position = new Vector3(worldPoint.x, piece.position.y, worldPoint.z);	
			Chess chess = piece.GetComponent<Chess>();
			chess.setFirstVector(piece.position);
			chess.setBeforeVector(piece.position);
			/*State state = piece.GetComponent<State>();
			state.setSetUp((int)demo[i]);
			state.setColor("black");
			*/
			chess.setSetUp((int)demo[i]);
			//chess.setColor("black");
			boardState.chessBoardArray[5,i] = piece.gameObject;
			chess.boardState = boardState;
			chess.setMaxI(6);
			chess.setMaxJ(5);
		}
		for (int i = 0; i < 5; i++) {
			Transform piece = GameObject.Instantiate (whitePiecePrefabs [(int)Piece.Pawn]);
			Vector3 worldPoint = ToWorldPoint (i+8);
			piece.position = new Vector3(worldPoint.x, piece.position.y, worldPoint.z);
			Chess chess = piece.GetComponent<Pawn>();
			chess.setFirstVector(piece.position);
			chess.setBeforeVector(piece.position);
			/*State state = piece.GetComponent<State>();
			state.setSetUp((int)Piece.Pawn);
			state.setColor("white");
			*/
			chess.setSetUp((int)Piece.Pawn);
			//chess.setColor("white");
			boardState.chessBoardArray[1,i] = piece.gameObject;
			chess.boardState = boardState;
			chess.setMaxI(6);
			chess.setMaxJ(5);
		}
		
		for (int i = 0; i < 5; i++) {
			Transform piece = GameObject.Instantiate (blackPiecePrefabs [(int)Piece.Pawn]);
			Vector3 worldPoint = ToWorldPoint (i+8*4);
			piece.position = new Vector3(worldPoint.x, piece.position.y, worldPoint.z);	
			Chess chess = piece.GetComponent<Pawn>();
			chess.setFirstVector(piece.position);
			chess.setBeforeVector(piece.position);
			/*State state = piece.GetComponent<State>();
			state.setSetUp((int)Piece.Pawn);
			state.setColor("black");
			*/
			chess.setSetUp((int)Piece.Pawn);
			//chess.setColor("black");
			boardState.chessBoardArray[4,i] = piece.gameObject;
			chess.boardState = boardState;
			chess.setMaxI(6);
			chess.setMaxJ(5);
		}
	}

	public static string GetCellString (int cellNumber)
	{
		int j = cellNumber % 8;
		int i = cellNumber / 8;
		return char.ConvertFromUtf32 (j + 65) + "" + (i + 1);
	}

	public static Vector3 ToWorldPoint(int cellNumber) {
		int j = cellNumber % 8;
		int i = cellNumber / 8;
		return new Vector3 (i*-4+14, 1, j*4-14);
	}
}
