import * as React from "react";
import { Project } from '../../types/store';
import { Card, CardHeader, CardTitle, CardBody } from 'reactstrap';

export interface Props {
    projects?: Project[];
    loadProjects: () => void;
}

export default class extends React.Component<Props> {
    constructor(props: Props) {
        super(props);
    }

    componentDidMount() {
        this.props.loadProjects();
    }

    render() {
        const { projects } = this.props;
        return (
            <Card>
                <CardHeader>
                    <CardTitle tag="h4">Projects</CardTitle>
                </CardHeader>
                <CardBody>

                    <div>Projects Data
                        {projects && "Count: " + projects.length}
                    </div>

                </CardBody>
            </Card>
        );
    }
}