import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  eventos: IEventos[];
  title = 'ProAgil Eventos !';

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.GetEventos();
  }


  GetEventos() {
    this.http.get<IEventos[]>('http://localhost:5000/api/values').subscribe(
        response =>  {
          this.eventos = response;
        }, error => {
          console.log(error);
        });
  }

}

export interface IEventos {
  eventoId: int;
  tema: string;
  local: string;
}
