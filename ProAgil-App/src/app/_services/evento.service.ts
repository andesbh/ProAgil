import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Evento } from '../_models/Evento';

@Injectable({
  // Primeira forma para permitir injetar este componente em todo o projeto
  providedIn: 'root'
})
export class EventoService {

  baseURL = 'http://localhost:5000/api/Evento';

  constructor(private http: HttpClient) { }

  getAllEventos(): Observable<Evento[]> {
    return this.http.get<Evento[]>(this.baseURL);
  }

  getEventoByTema(tema: string): Observable<Evento[]> {
    return this.http.get<Evento[]>(`${this.baseURL}/getByTema/${tema}`);
  }

  getEventoById(Id: number): Observable<Evento> {
    return this.http.get<Evento>(`${this.baseURL}/${Id}`);
  }

}
