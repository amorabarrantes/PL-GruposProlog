verificarNumero([X,Y]):- punto(X,Y),!.

vecinos([X, Y],[X2,Y2]) :-
	punto(X,Y), punto(X2,Y2),
	A is X2-1, B is X2+1, C is Y2 -1, D is Y2+1,
	((X = X2, Y = D);(X = X2, Y = C);(Y = Y2, X = B);(Y = Y2, X = A)).

grupo(X,Y,[],_):- vecinos(X,Y).
grupo(X,Y,[Alguno|Lista], Visitados):- vecinos(X, Alguno),
	not(member(Alguno, Visitados)),
	grupo(Alguno, Y, Lista, [Alguno|Visitados]).

final(Comprobacion,A,B) :- consult('baseDeDatos.pl'),verificarNumero(Comprobacion),findall(B, grupo(A,B,_,[]),Lista), sort(Lista,B).





