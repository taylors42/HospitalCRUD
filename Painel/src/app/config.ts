export interface Entidade {
  key?: string;
  nome: string;
  sobrenome: string;
  dataDeNascimento: string;
  genero: string;
  cpf?: string;
  rg: string;
  rguf: string;
  email: string;
  celular?: string;
  telefoneFixo?: string;
  convenio: string;
  numeroCarteirinha: string;
  validadeCarteirinha: string;
}

export interface CreatePacienteDTO {
  nome: string;
  sobrenome: string;
  dataDeNascimento: string;
  genero: string;
  cpf?: string;
  rg: string;
  rguf: string;
  email: string;
  celular?: string;
  telefoneFixo?: string;
  convenio: string;
  numeroCarteirinha: string;
  validadeCarteirinha: string;
}

export interface UpdatePacienteDTO {
  nome?: string;
  sobrenome?: string;
  dataDeNascimento?: string;
  genero?: string;
  email?: string;
  celular?: string;
  telefoneFixo?: string;
  convenio?: string;
  numeroCarteirinha?: string;
  validadeCarteirinha?: string;
  // RG, RGUF e CPF não podem ser alterados após o cadastro
}

export interface Convenio {
  id: number;
  nome: string;
}

export const API_CONFIG = {
  baseUrl: 'http://localhost:3000',
  endpointPaciente: 'Paciente',
  endpointConvenio: 'convenios'
};

export const LABELS = {
  titulo: 'Gerenciador de Pacientes',
  tituloFormularioAdicionar: 'Adicionar Paciente',
  tituloFormularioEditar: 'Editar Paciente',
  btnSalvar: 'Salvar',
  btnAtualizar: 'Atualizar',
  btnCancelar: 'Cancelar',
  btnEditar: 'Editar',
  btnDeletar: 'Deletar',
  mensagemVazia: 'Nenhum paciente cadastrado ainda.',
  labelNome: 'Nome',
  labelSobrenome: 'Sobrenome',
  labelDataNascimento: 'Data de Nascimento',
  labelGenero: 'Gênero',
  labelCpf: 'CPF',
  labelRg: 'RG',
  labelRgUf: 'UF do RG',
  labelEmail: 'Email',
  labelCelular: 'Celular',
  labelTelefoneFixo: 'Telefone Fixo',
  labelConvenio: 'Convênio',
  labelNumeroCarteirinha: 'Número da Carteirinha',
  labelValidadeCarteirinha: 'Validade da Carteirinha (MM/AAAA)',
  colunaId: 'ID',
  colunaNome: 'Nome',
  colunaSobrenome: 'Sobrenome',
  colunaEmail: 'Email',
  colunaConvenio: 'Convênio',
  colunaNumeroCarteirinha: 'Nº Carteirinha',
  colunaValidadeCarteirinha: 'Validade',
  colunaAcoes: 'Ações',
};

export const MENSAGENS = {
  criado: 'Paciente criado com sucesso!',
  atualizado: 'Paciente atualizado com sucesso!',
  deletado: 'Paciente deletado com sucesso!',
  erroCarregar: 'Erro ao carregar pacientes. Verifique a conexão com a API.',
  erroCriar: 'Erro ao criar paciente.',
  erroAtualizar: 'Erro ao atualizar paciente.',
  erroDeletar: 'Erro ao deletar paciente.',
  confirmarDelete: 'Tem certeza que deseja deletar este paciente?',
};

export function getPacienteApiUrl(): string {
  return `${API_CONFIG.baseUrl}/${API_CONFIG.endpointPaciente}`;
}

export function getConvenioApiUrl(): string {
  return `${API_CONFIG.baseUrl}/${API_CONFIG.endpointConvenio}`;
}

export function getApiUrl(): string {
  return getPacienteApiUrl();
}
