import { createStore, applyMiddleware } from "redux";
import thunkMiddleware from "redux-thunk";
import { rootReducer } from './root.reducer';
import { UserList } from '../modules/users/types/store';
import { ProjectList } from '../modules/projects/types/store';

export const store = createStore(
    rootReducer,
    applyMiddleware(thunkMiddleware)
)

export interface AppStore {
    userList: UserList;
    projectList: ProjectList
}

