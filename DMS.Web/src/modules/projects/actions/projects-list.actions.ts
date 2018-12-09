import * as constants from "../constants/projects-list.constants";
import { Project } from '../types/store';
import { projectsServices } from '../services/projects.service';
import { Dispatch } from 'react';

export const projectListActions = {
    loadProjects
}

export interface LoadProjectList {
    type: constants.LOAD_PROJECT_LIST;
    projects: Project[]
}

export type ProjectListActions = LoadProjectList;

function loadProjects() {
    return (dispatch: Dispatch<ProjectListActions>) => {
    projectsServices.loadProjects()
        .then((projects) => {
            dispatch({type: constants.LOAD_PROJECT_LIST, projects});
        })
    }
}