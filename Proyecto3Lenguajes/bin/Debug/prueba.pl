persona(carlos).
persona(ana).

cargar(A) :-  exists_file(A),consult(A).
familia(X) :- persona(X).

