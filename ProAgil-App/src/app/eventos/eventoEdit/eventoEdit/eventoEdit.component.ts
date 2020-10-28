import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { ToastrService } from 'ngx-toastr';
import { DateTimeFormatPipePipe } from 'src/app/_helps/DateTimeFormatPipe.pipe';
import { Evento } from 'src/app/_models/Evento';
import { EventoService } from 'src/app/_services/evento.service';
import { MensagensServiceService } from 'src/app/_services/mensagensService.service';

@Component({
  selector: 'app-evento-edit',
  templateUrl: './eventoEdit.component.html',
  styleUrls: ['./eventoEdit.component.scss']
})
export class EventoEditComponent implements OnInit {

  titulo = 'Editar Evento';
  registerForm: FormGroup;
  evento: Evento = new Evento();
  imagemURL = 'assets/imagens/upload-picture-icon-18.jpg';
  imagemToUpdate: string;
  dataAtual;

  get lotes(): FormArray {
    return this.registerForm.get('lotes') as FormArray;
  }

  get redesSociais(): FormArray {
    return this.registerForm.get('redesSociais') as FormArray;
  }

  constructor(private eventoService: EventoService, private fb: FormBuilder,
              private localeService: BsLocaleService, private DateTimeFormat: DateTimeFormatPipePipe,
              private Mensagens: MensagensServiceService, private toastr: ToastrService,
              private router: ActivatedRoute) {
      this.localeService.use('pt-br');
    }

    ngOnInit() {
      this.validation();
      this.carregarEvento();
    }

  carregarEvento() {
    const idEvento: number = +this.router.snapshot.paramMap.get('id');
    this.eventoService.getEventoById(idEvento)
                .subscribe(
                  (evento: Evento) => {
                    this.evento = Object.assign({}, evento);
                    this.imagemToUpdate = evento.imagemUrl.toString();
                    this.imagemURL = `http://localhost:5000/Resources/Images/${this.evento.imagemUrl}?_ts=${this.dataAtual}`;
                    this.evento.imagemUrl = '';
                    this.registerForm.patchValue(this.evento);
                  }
                );
  }

    validation() {
      this.registerForm = this.fb.group({
        tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
        local: ['', Validators.required],
        dataEvento: ['', Validators.required],
        quantidadeDePessoas: ['', [Validators.required, Validators.max(150)]],
        imagemURL: [''],
        telefone: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        lotes: this.fb.array([this.criaLote()]),
        redesSociais: this.fb.array([this.criaRedeSocial()]) ,
      });
    }

    criaLote(): FormGroup {
      return this.fb.group({
        nome: ['', Validators.required],
        quantidade: ['', Validators.required],
        preco: ['', Validators.required],
        dataInicio: ['', Validators.required],
        dataFim: ['', Validators.required]
      });
    }

    criaRedeSocial(): FormGroup {
      return this.fb.group({
        nome: ['', Validators.required],
        url: ['', Validators.required],
      });
    }

    onFileChange(event: any) {

      const reader = new FileReader();
      reader.onload = (strURL: any) => this.imagemURL = strURL.target.result;

      reader.readAsDataURL(event.target.files[0]);
    }

    adicionarLote() {
      this.lotes.push(this.criaLote());
    }

    adicionarRedeSocial() {
      this.redesSociais.push(this.criaRedeSocial());
    }

    removerLote(id: number) {
      this.lotes.removeAt(id);
    }

    removerRedeSocial(id: number) {
      this.redesSociais.removeAt(id);
    }
}


