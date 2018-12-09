import { apiService } from "../../shared/services/api.service";
import { Project } from '../types/store';

export const projectsServices = {
    loadProjects
}

function loadProjects() {
    return apiService.get<Project[]>("projects");
}