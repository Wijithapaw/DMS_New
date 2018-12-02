import { createStore, applyMiddleware } from "redux";
import thunkMiddleware from "redux-thunk";
import { rootReducer } from './root.reducer';
import { UserList } from '../modules/users/types/store';

export const store = createStore(
    rootReducer,
    applyMiddleware(thunkMiddleware)
)

export interface Store {
    userList?: UserList
}

