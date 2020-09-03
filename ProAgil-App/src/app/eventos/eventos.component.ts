import { Component, OnInit, ViewChild, ElementRef, TemplateRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_models/Evento';
import { BsModalService } from 'ngx-bootstrap/modal';
import { FormGroup, Validators, FormBuilder, FormControl } from '@angular/forms';
import { defineLocale, ptBrLocale } from 'ngx-bootstrap/chronos';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { DateTimeFormatPipePipe } from '../_helps/DateTimeFormatPipe.pipe';
import { Constantes } from '../Util/Constantes';
import { MensagensServiceService } from '../_services/mensagensService.service';
import { ToastrService } from 'ngx-toastr';

defineLocale('pt-br', ptBrLocale);

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
  eventos: Evento[];
  evento: Evento;
  eventosFiltrados: Evento[] = null;
  imagemLargura = 50;
  imagemMargem = 2;
  mostrarImagem = true;
  registerForm: FormGroup;
  editando = false;
  bodyDeletarEvento: string;

  titulo = 'Eventos';
  file: File;
  imagemToUpdate: string;
  dataAtual: string;
  constructor(private eventoService: EventoService, private modalService: BsModalService, private fb: FormBuilder,
              private localeService: BsLocaleService, private DateTimeFormat: DateTimeFormatPipePipe,
              private Mensagens: MensagensServiceService, private toastr: ToastrService) {
    this.localeService.use('pt-br');
  }

  public get filtroLista(): string {
    return this._filtroLista;
  }
  public set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista === '' ? this.eventos : this.filtrarEvento(this.filtroLista);
  }
  ngOnInit() {
    this.validation();
    this.GetEventos();
  }
  AlternarImagem() {
    this.mostrarImagem = !this.mostrarImagem;
  }
  GetEventos() {
    // tslint:disable-next-line: variable-name
    this.eventoService.getAllEventos().subscribe((_eventos: Evento[]) =>  {
                                    this.eventos = _eventos;
                                  }, error => {
                                    this.toastr.error(`Erro ao carregar os registros. Erro: ${error}`, 'Proagil');
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

    editarEvento(template: any, evento: Evento) {
      this.editando = true;
      this.evento = Object.assign({}, evento);
      this.imagemToUpdate = this.evento.imagemUrl.toString();
      this.evento.imagemUrl = '';
      this.registerForm.patchValue(this.evento);
      this.openModal(template, this.evento);
    }

    novoEvento(template: any) {
      this.editando = false;
      this.openModal(template);
    }

    openModal(template: any, evento?: any) {
      this.registerForm.reset();
      template.show(template);
      if (evento !== undefined) {
        this.evento = evento;
        this.registerForm.patchValue(evento);
      }
    }
    validation() {
      this.registerForm = this.fb.group({
        tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
        local: ['', Validators.required],
        dataEvento: ['', Validators.required],
        quantidadeDePessoas: ['', [Validators.required, Validators.max(150)]],
        imagemUrl: ['', Validators.required],
        telefone: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]]
      });
    }

    salvarAlteracao(template: any) {
      if (this.registerForm.valid) {
        this.evento = Object.assign({}, this.registerForm.value);

        this.uploadImagem(false);
        this.eventoService.PostEvento( this.evento)
                            .subscribe((evento: Evento) => {
                              console.log(evento);
                              this.GetEventos();
                              this.toastr.success('Dados inseridos com sucesso!', 'ProAgil');
                              },
                              error => {
                                console.log(error);
                                this.toastr.error(`Erro ao salvar. Erro: ${error}`, 'Proagil');
                               });
        template.hide();
      }
    }
    editarAlteracao(template: any) {
      if (this.registerForm.valid) {
        this.evento = Object.assign({id: this.evento.id}, this.registerForm.value);

        this.uploadImagem(true);
        this.eventoService.PutEvento(this.evento)
                    .subscribe((evento: Evento) => {
                      console.log(evento);
                      this.GetEventos();
                      this.toastr.success('Dados atualizados com sucesso!', 'ProAgil');
                    }, error => {
                      console.log(error);
                      this.toastr.error(`Erro ao atualizar. Erro: ${error}`, 'Proagil');
                    });
        template.hide();
      }
    }

    uploadImagem(update: boolean) {

      let nomeArquivo: string;
      if (update) {
        nomeArquivo = this.imagemToUpdate;
        console.log(nomeArquivo);
        this.registerForm.removeControl('imgUpdate');
      } else {
        nomeArquivo = this.evento.imagemUrl.split('\\', 3)[2];
      }
      this.evento.imagemUrl = nomeArquivo;
      this.eventoService.postUpload(this.file, nomeArquivo)
        .subscribe(
          () => { this.dataAtual = new Date().getMilliseconds().toString();
                  this.GetEventos(); }
        );
    }
    excluirEvento(evento: Evento, template: any) {
      template.show(template);
      this.evento = evento;
      this.bodyDeletarEvento = `Tem certeza que deseja excluir o Evento: ${evento.tema}, Código: ${evento.id}`;
    }
    confirmeDelete(template: any) {
      this.eventoService.DeleteEvento(this.evento).subscribe(
        () => {
            template.hide();
            this.GetEventos();
            this.toastr.success('Dados deletados com sucesso!', 'ProAgil');
          }, error => {
            console.log(error);
            this.toastr.error(`Erro ao deletar o registro. Erro: ${error}`, 'Proagil');
          }
      );
    }
    onFileChange(event: any) {

      const reader = new FileReader();

      if (event.target.files && event.target.files.length){
        this.file = event.target.files;
      }

    }
  }
