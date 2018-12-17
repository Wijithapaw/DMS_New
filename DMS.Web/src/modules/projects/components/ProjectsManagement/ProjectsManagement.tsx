import * as React from "react";
import { Project } from '../../types/store';
import { Card, CardHeader, CardTitle, CardBody } from 'reactstrap';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { Button } from 'primereact/button';

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
                    <Button label="Text Button" icon="pi pi-check" />
                    <div>Projects Data
                        {projects && "Count: " + projects.length}
                    </div>
                    <DataTable value={this.props.projects} sortField="title" sortOrder={1}>
                        <Column field="title" header="Title" sortable={true} />
                        <Column field="description" header="Description" sortable={true} />
                        <Column field="projectCategory" header="Category" sortable={true} />
                        <Column field="startDate" header="Start Data" sortable={true} />
                        <Column field="endDate" header="End Data" sortable={true} />
                    </DataTable>
                </CardBody>
            </Card>
        );
    }
}