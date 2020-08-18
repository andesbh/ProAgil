import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MensagensServiceService {

constructor() { }
  confimaDelete() {
    return 'Confirma a exclusão do item selecionado?';
  }
}
