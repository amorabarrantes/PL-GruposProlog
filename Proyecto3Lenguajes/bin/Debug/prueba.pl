start :- dynamic(conexion/2),
         consult('BaseDeDatos.pl').

comprobar(X,Y) :-
	 dynamic(conexion/2),
         consult('BaseDeDatos.pl'),
	 conexion(X,Y),
	 conocer(1,2).

conocimiento(1,2).

conocer(X,Y) :- conocimiento(X,Y).

