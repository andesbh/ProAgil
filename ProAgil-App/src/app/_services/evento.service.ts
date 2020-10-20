import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Evento } from '../_models/Evento';

@Injectable({
  // Primeira forma para permitir injetar este componente em todo o projeto
  providedIn: 'root'
})
export class EventoService {

  baseURL = 'http://localhost:5000/api/Evento';

  constructor(private http: HttpClient) {}

  getAllEventos(): Observable<Evento[]> {
    return this.http.get<Evento[]>(this.baseURL);
  }

  getEventoByTema(tema: string): Observable<Evento[]> {
    return this.http.get<Evento[]>(`${this.baseURL}/getByTema/${tema}`);
  }

  getEventoById(Id: number): Observable<Evento> {
    return this.http.get<Evento>(`${this.baseURL}/${Id}`);
  }

  PostEvento(evento: Evento): Observable<Evento> {
    return this.http.post<Evento>(this.baseURL, evento);
  }

  PutEvento(evento: Evento) {
    return this.http.put(`${this.baseURL}/${evento.id}`, evento);
  }

  DeleteEvento(evento: Evento) {
    return this.http.delete(`${this.baseURL}/${evento.id}`);
  }

  postUpload(file: File, fileName: string): Observable<any> {

    const fileToUpload = file[0] as File;
    const formData = new FormData();

    formData.append('file', fileToUpload, fileName);
    return this.http.post<any>(`${this.baseURL}/upload`, formData);
  }

}
