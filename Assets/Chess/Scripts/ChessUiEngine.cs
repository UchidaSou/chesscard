using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessUiEngine : MonoBehaviour {
	enum Piece {King=0, Queen=1, Rook=2, Knight=3, Bishop=4, Pawn=5};
	private Piece[] setup = new Piece[] {Piece.Rook, Piece.Knight, Piece.Bishop, Piece.Queen, Piece.King, Piece.Bishop, Piece.Knight, Piece.Rook};
	public BoxCollider bounds;
	public List<Transform> whitePiecePrefabs;
	public List<Transform> blackPiecePrefabs;

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
		for (int i = 0; i < 8; i++) {
			Transform piece = GameObject.Instantiate (whitePiecePrefabs [(int)setup [i]]);
			Vector3 worldPoint = ToWorldPoint (i);
			piece.position = new Vector3(worldPoint.x, piece.position.y, worldPoint.z);	
			Chess chess = piece.GetComponent<Chess>();
			chess.setFirstVector(piece.position);
			chess.setBeforeVector(piece.position);
			State state = piece.GetComponent<State>();
			state.setSetUp((int)setup[i]);
			state.setColor("white");
		}
		for (int i = 0; i < 8; i++) {
			Transform piece = GameObject.Instantiate (blackPiecePrefabs [(int)setup [i]]);
			Vector3 worldPoint = ToWorldPoint (i+56);
			piece.position = new Vector3(worldPoint.x, piece.position.y, worldPoint.z);	
			Chess chess = piece.GetComponent<Chess>();
			chess.setFirstVector(piece.position);
			chess.setBeforeVector(piece.position);
			State state = piece.GetComponent<State>();
			state.setSetUp((int)setup[i]);
			state.setColor("black");
			
		}
		for (int i = 0; i < 8; i++) {
			Transform piece = GameObject.Instantiate (whitePiecePrefabs [(int)Piece.Pawn]);
			Vector3 worldPoint = ToWorldPoint (i+8);
			piece.position = new Vector3(worldPoint.x, piece.position.y, worldPoint.z);
			Chess chess = piece.GetComponent<Pawn>();
			chess.setFirstVector(piece.position);
			chess.setBeforeVector(piece.position);
			State state = piece.GetComponent<State>();
			state.setSetUp((int)Piece.Pawn);
			state.setColor("white");
			
		}
		
		for (int i = 0; i < 8; i++) {
			Transform piece = GameObject.Instantiate (blackPiecePrefabs [(int)Piece.Pawn]);
			Vector3 worldPoint = ToWorldPoint (i+48);
			piece.position = new Vector3(worldPoint.x, piece.position.y, worldPoint.z);	
			Chess chess = piece.GetComponent<Pawn>();
			chess.setFirstVector(piece.position);
			chess.setBeforeVector(piece.position);
			State state = piece.GetComponent<State>();
			state.setSetUp((int)Piece.Pawn);
			state.setColor("black");
			
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
	public static Chess chessGetComponent(GameObject gameObject){
		if(gameObject == null){
			return null;
		}
        State state = gameObject.GetComponent<State>();
        int setup = state.getSetUp();
        Chess chess;
        switch(setup){
            case 0:
                chess = (Chess)gameObject.GetComponent<King>();
                return chess;
            case 1:
                chess = (Chess)gameObject.GetComponent<Queen>();
                return chess;
            case 2:
                chess = (Chess)gameObject.GetComponent<Rook>();
                return chess;
            case 3:
                chess = (Chess)gameObject.GetComponent<Knight>();
                return chess;
            case 4:
                chess = (Chess)gameObject.GetComponent<Bishop>();
                return chess;
            case 5:
                chess = (Chess)gameObject.GetComponent<Pawn>();
                return chess;
        }
        return null;
    }

	
}
