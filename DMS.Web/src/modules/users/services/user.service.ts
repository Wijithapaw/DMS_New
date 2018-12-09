import { apiService } from "../../shared/services/api.service";
import { User } from '../types/store';

export const userService = {
    getAll
}

function getAll() {
    return apiService.get<User[]>('users');
}