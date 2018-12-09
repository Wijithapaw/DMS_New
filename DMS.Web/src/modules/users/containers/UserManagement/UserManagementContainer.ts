import { AppStore } from '../../../../redux/store';
import { userActions } from '../../actions/user-list.actions';
import { connect } from 'react-redux';
import { UserManagement } from '../../components';

const mapStateToProps = (store: AppStore) => {
    const { users } = store.userList;
    return { users };
}

const mapDispatchToProps = (dispatch: any) => {
    return {
        loadData: () => dispatch(userActions.loadUserList())
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(UserManagement)
