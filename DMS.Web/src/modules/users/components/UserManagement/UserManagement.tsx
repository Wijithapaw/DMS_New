import * as React from "react";
import {
  Card,
  CardBody,
  CardHeader,
  CardTitle,
} from "reactstrap";
import { User } from '../../types/store';

export interface Props {
  users?: User[];
  loadData: () => void;
}

class UserManagement extends React.Component<Props> {

  componentDidMount() {
    this.props.loadData();
  }

  render() {
    return (
      <Card>
        <CardHeader>
          <CardTitle tag="h4">Simple Table</CardTitle>
        </CardHeader>
        <CardBody>

          {this.props.users && this.props.users[0].firstName}
          User Data

                  {this.props.users && this.props.users.forEach((user: User) => {
            { user.firstName }
          })
          }

        </CardBody>
      </Card>
    );
  }
}

export default UserManagement;
