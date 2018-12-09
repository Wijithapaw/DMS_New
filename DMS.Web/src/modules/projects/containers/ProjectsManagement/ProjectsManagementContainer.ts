import { AppStore } from "../../../../redux/store";
import { projectListActions } from '../../actions/projects-list.actions';
import { connect } from 'react-redux';
import ProjectsManagement from '../../components/ProjectsManagement/ProjectsManagement';

const mapStateToProps = (store: AppStore) => {
    const {projects} = store.projectList;
    return { projects };
}

const mapDispachToProps = (dispatch: any) => {
    return {
        loadProjects: () => dispatch(projectListActions.loadProjects())
    }
}

export default connect(mapStateToProps, mapDispachToProps)(ProjectsManagement)