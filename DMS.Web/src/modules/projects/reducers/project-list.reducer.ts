import { ProjectList } from "../types/store";
import { ProjectListActions } from '../actions/projects-list.actions';
import * as constants from '../constants/projects-list.constants'

export default function projectList(state: ProjectList = {}, action: ProjectListActions) {
    switch(action.type) {
        case constants.LOAD_PROJECT_LIST: {
            return { ...state, projects: action.projects }
        }
    }
    return state;
}