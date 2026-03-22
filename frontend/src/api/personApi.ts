import type {
  CreatePersonRequest,
  Person,
  PersonTotalsResult,
  UpdatePersonRequest,
} from '../types/person'
import api from './axios'

export async function getPersons(): Promise<Person[]> {
  const { data } = await api.get<Person[]>('/api/persons')
  return data
}

export async function createPerson(body: CreatePersonRequest): Promise<Person> {
  const { data } = await api.post<Person>('/api/persons', body)
  return data
}

export async function updatePerson(
  id: string,
  body: UpdatePersonRequest,
): Promise<Person> {
  const { data } = await api.put<Person>(`/api/persons/${id}`, body)
  return data
}

export async function deletePerson(id: string): Promise<void> {
  await api.delete(`/api/persons/${id}`)
}

export async function getPersonTotals(): Promise<PersonTotalsResult> {
  const { data } = await api.get<PersonTotalsResult>('/api/persons/totals')
  return data
}
