import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ToastrModule } from 'ngx-toastr';

import { AppComponent } from './app.component';
import { EventosComponent } from './eventos/eventos.component';
import { NavComponent } from './nav/nav.component';
import { DashboardComponent } from './dashboard/dashboard/dashboard.component';
import { ContatosComponent } from './contatos/contatos/contatos.component';
import { PalestrantesComponent } from './palestrantes/palestrantes/palestrantes.component';
import { TituloComponent } from './_shared/titulo/titulo.component';

import { DateTimeFormatPipePipe } from './_helps/DateTimeFormatPipe.pipe';
import { EventoService } from './_services/evento.service';




@NgModule({
   declarations: [
      AppComponent,
      EventosComponent,
      NavComponent,
      DashboardComponent,
      ContatosComponent,
      PalestrantesComponent,
      TituloComponent,
      DateTimeFormatPipePipe
   ],
   imports: [
      BrowserModule,
      AppRoutingModule,
      HttpClientModule,
      FormsModule,
      BsDatepickerModule.forRoot(),
      ModalModule.forRoot(),
      TooltipModule.forRoot(),
      BsDropdownModule.forRoot(),
      BrowserAnimationsModule,
      ReactiveFormsModule,
      ToastrModule.forRoot({
         timeOut: 2000,
         preventDuplicates: true,
       }),
   ],
   providers: [
      // Terceira forma de injetar o serviço para ser utilizado em todo o projeto
      EventoService,
      DateTimeFormatPipePipe
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
