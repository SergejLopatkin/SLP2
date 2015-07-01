using UnityEngine;
using System.Collections;

public class Chessboard : MonoBehaviour {
	
	GameObject[,] m_Grid;
	int[,] m_KopierteArraymitAnzahlNachbar;
	public int m_iSize = 10;
	public int m_iRangeOfQuad = 1;
	public Color m_ColorOfQuadsAlive = Color.blue;  // Die Farbe der Kacheln die "leben".
	public Color m_ColorOfQuadsDead = Color.white;	// Die Farbe der Kacheln die "tot" sind.

	// Use this for initialization
	void Start () {
		m_Grid = new GameObject[m_iSize, m_iSize];
		m_KopierteArraymitAnzahlNachbar = new int[m_iSize, m_iSize];

																					// Erstellen Kachel und Zuweisung als child von einer Parent-Kachel.
		for (int i = 0; i < m_iSize; i++) {
			for (int j = 0; j < m_iSize; j++) {
				GameObject kachel = GameObject.CreatePrimitive (PrimitiveType.Quad);
				m_Grid [i, j] = kachel;
				kachel.name = "Kachel[" + i + "][" + j + "]";
				kachel.transform.position = new Vector3 (i, j, 0);
				kachel.transform.parent = this.transform;
			}
		}
		Camera.main.transform.position = new Vector3 (m_iSize / 2, m_iSize / 2, -100);
		Camera.main.orthographicSize = m_iSize;

		transform.position = new Vector3 (0.5f, 0.5f, 0); 
						

	}
	//Color ChangeColorOfGameobject (Array,int,int,Color)
																					// Random umkolorieren von allen Kacheln
	void RandomUmfärben(){
			for (int i = 0; i < m_iSize; i++) {
			
			for (int j = 0; j < m_iSize; j++) {
				float iBetween0to1 = Random.value;
				if(iBetween0to1 <= 0.5){
					m_Grid[i,j].GetComponent<Renderer>().material.color = m_ColorOfQuadsAlive;
				}else m_Grid[i,j].GetComponent<Renderer>().material.color = m_ColorOfQuadsDead;
				
				}
			}
		}

	 																				// Vergleicht : Ob eine Kachel lebt oder tot ist.
	bool GetAlive(int _iCol,int _iRow){
		return(m_Grid[_iCol, _iRow].GetComponent<Renderer>().material.color == m_ColorOfQuadsAlive);
	}
	void ArrayImSpeicher()
	{
		for (int i = 0; i < m_iSize; i++) {
			for (int j = 0; j < m_iSize; j++) {
				int iTmp = GetAliveNeighbours(i,j,m_iRangeOfQuad);
				m_KopierteArraymitAnzahlNachbar[i,j] = iTmp;
			}
		}
	}
																					// Die Generation wird mit den aktuellen Regeln geupdatet.
	void GenerationsUpdate(GameObject[,] _Array) {
		for (int iCollum = 0; iCollum < m_iSize; iCollum++) {
			for (int iRow = 0; iRow < m_iSize; iRow++) {
				
				int iNeigh = m_KopierteArraymitAnzahlNachbar[iCollum,iRow];
				// tile dead
				if (!GetAlive(iCollum,iRow))
				{ 
					if(iNeigh == 3)
						_Array[iCollum,iRow].GetComponent<Renderer>().material.color = m_ColorOfQuadsAlive;
				}
				// tile alive
				else 
				{
						if(iNeigh>3 || iNeigh<2)
						{
							_Array[iCollum,iRow].GetComponent<Renderer>().material.color = m_ColorOfQuadsDead;
						}
				}
			}
		}
	}
																					// Prüft wie viele Nachbarm eine Kachel hat. 	o o o
	int GetAliveNeighbours(int _iRow,int _iCollum,int _iRange)						//												o x o 
	{																				//												o o o
		int iAliveNeighboursCounter = 0;
		for (int iColDelta = -_iRange; iColDelta <= _iRange; iColDelta++)
			for (int iRowDelta = -_iRange; iRowDelta <= _iRange; iRowDelta++) {
			int iNeighRow = _iRow + iRowDelta;
			int iNeighCol = _iCollum + iColDelta;
			if(iNeighCol == _iCollum && iNeighRow == _iRow) 								    // don't check yourself (only neighs)
				continue;
			if (iNeighCol >= 0 && iNeighCol < m_iSize && iNeighRow >= 0 && iNeighRow < m_iSize // check array bounds									
			    && GetAlive(iNeighRow,iNeighCol))											   // count alive
						iAliveNeighboursCounter++;
			}

		/*int iAliveNeighboursCounter = 0;  // FAIL CODE
		for (int i = -1; i <= 1; i++) {
			int iTmp = i+_iCollum;
			for(int j = -1;j<= 1;j++){
				int jTmp = j + _iRow;
				if(!(iTmp == _iCollum && jTmp == _iRow) && !(iTmp < 0) && !(jTmp < 0) && !(iTmp > m_iSize-1) && !(jTmp > m_iSize-1) )
					if(m_Grid[iTmp,jTmp].GetComponent<Renderer>().material.color == Color.blue)
						iAliveNeighboursCounter++;
				
			}
			
			
		}  */  
		return iAliveNeighboursCounter;
	}

	void KillAll(){
		for (int i = 0; i < m_iSize; i++) {
			for (int j = 0; j < m_iSize; j++) {
				m_Grid[i,j].GetComponent<Renderer>().material.color = m_ColorOfQuadsDead;
			}
		}
	}
	void SetAliveOrDead (int _iCol,int _iRow,bool bAlive){
		if (bAlive == true)
			m_Grid[_iCol, _iRow].GetComponent<Renderer> ().material.color = m_ColorOfQuadsAlive;
		else
			m_Grid[_iCol, _iRow].GetComponent<Renderer> ().material.color = m_ColorOfQuadsDead;
	}
	void Toggle(int _iCol,int _iRow){
		bool bAlive = GetAlive (_iCol, _iRow);
		SetAliveOrDead (_iCol, _iRow, !bAlive);
		/*if (GetAlive(_iCol,_iRow) == false)
			m_Grid [_iCol, _iRow].GetComponent<Renderer> ().material.color = m_ColorOfQuadsAlive;
		else {
			m_Grid [_iCol, _iRow].GetComponent<Renderer> ().material.color = m_ColorOfQuadsDead;
		}*/
	}
	void ToggleMouseField(){
		//Debug.Log((int)Camera.main.ScreenToWorldPoint (Input.mousePosition));
		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		int xIndex = (int) mouseWorldPos.x;
		int yIndex = (int) mouseWorldPos.y;
		Toggle (xIndex, yIndex);
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0))
			ToggleMouseField ();

		if(Input.GetKey(KeyCode.RightArrow)){
			RandomUmfärben();
		}
		if(Input.GetKey(KeyCode.Space)){
			ArrayImSpeicher();
			GenerationsUpdate (m_Grid);
		}
		if (Input.GetKeyUp (KeyCode.DownArrow)) {
			ArrayImSpeicher();
			GenerationsUpdate (m_Grid);
		}
		if(Input.GetKeyDown(KeyCode.UpArrow))
		   KillAll();
		
	}
}
