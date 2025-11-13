import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Pacientes } from './pacientes/pacientes';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Pacientes],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('Painel');
}
