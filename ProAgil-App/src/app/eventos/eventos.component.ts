import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_models/Evento';


@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
  // Segunda forma de injetar o servico para ser utilizado somente neste local
  // providers: [EventoService]
})
export class EventosComponent implements OnInit {
  // tslint:disable-next-line: variable-name
  private _filtroLista: string;
  public get filtroLista(): string {
    return this._filtroLista;
  }
  public set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista === '' ? this.eventos : this.filtrarEvento(this.filtroLista);
  }

  eventos: Evento[];
  eventosFiltrados: Evento[] = null;
  imagemLargura = 50;
  imagemMargem = 2;
  mostrarImagem = true;

  constructor(private eventoService: EventoService) { }

  ngOnInit() {
    this.GetEventos();
  }
  AlternarImagem() {
    this.mostrarImagem = !this.mostrarImagem;
  }
  GetEventos() {
    this.eventoService.getAllEventos().subscribe(
    // tslint:disable-next-line: variable-name
    (_eventos: Evento[]) =>  {
      this.eventos = _eventos;
    }, error => {
      console.log(error);
    });
  }
  filtrarEvento(filtroLista: string): Evento[] {
    return this.eventos.filter(
      f => f.tema.toLocaleLowerCase().indexOf(filtroLista.toLocaleLowerCase()) !== -1
      );
    }
    retornaEventos(): Evento[] {
      return this.eventosFiltrados === null ? this.eventos : this.eventosFiltrados;
    }
  }
