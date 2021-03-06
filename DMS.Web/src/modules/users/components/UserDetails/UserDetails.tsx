import * as React from "react";
import { Card, CardHeader, CardBody, Row, Col } from "reactstrap";

import { FormInputs } from "src/modules/shared/components";


class UserDetails extends React.Component {
  render() {
    return (
      <div>
        <div className="content">
          <Row>
            <Col xs={12}>
              <Card>
                <CardHeader>
                  <h5 className="title">Edit Profile</h5>
                </CardHeader>
                <CardBody>
                  <form>
                    <FormInputs
                      ncols={[
                        "col-md-5 pr-1",
                        "col-md-3 px-1",
                        "col-md-4 pl-1"
                      ]}
                      proprieties={[
                        {
                          label: "Company (disabled)",
                          inputProps: {
                            type: "text",
                            disabled: true,
                            defaultValue: "Creative Code Inc."
                          }
                        },
                        {
                          label: "Username",
                          inputProps: {
                            type: "text",
                            defaultValue: "michael23"
                          }
                        },
                        {
                          label: "Email address",
                          inputProps: {
                            type: "email",
                            placeholder: "Email"
                          }
                        }
                      ]}
                    />
                    <FormInputs
                      ncols={["col-md-6 pr-1", "col-md-6 pl-1"]}
                      proprieties={[
                        {
                          label: "First Name",
                          inputProps: {
                            type: "text",
                            placeholder: "First Name",
                            defaultValue: "Mike"
                          }
                        },
                        {
                          label: "Last Name",
                          inputProps: {
                            type: "text",
                            placeholder: "Last Name",
                            defaultValue: "Andrew"
                          }
                        }
                      ]}
                    />
                    <FormInputs
                      ncols={["col-md-12"]}
                      proprieties={[
                        {
                          label: "Address",
                          inputProps: {
                            type: "text",
                            placeholder: "Home Address",
                            defaultValue:
                              "Bld Mihail Kogalniceanu, nr. 8 Bl 1, Sc 1, Ap 09"
                          }
                        }
                      ]}
                    />
                    <FormInputs
                      ncols={[
                        "col-md-4 pr-1",
                        "col-md-4 px-1",
                        "col-md-4 pl-1"
                      ]}
                      proprieties={[
                        {
                          label: "City",
                          inputProps: {
                            type: "text",
                            defaultValue: "Bucharest",
                            placeholder: "City"
                          }
                        },
                        {
                          label: "Country",
                          inputProps: {
                            type: "text",
                            defaultValue: "Romania",
                            placeholder: "Country"
                          }
                        },
                        {
                          label: "Postal Code",
                          inputProps: {
                            type: "number",
                            placeholder: "ZIP Code"
                          }
                        }
                      ]}
                    />
                    <FormInputs
                      ncols={["col-md-12"]}
                      proprieties={[
                        {
                          label: "About Me",
                          inputProps: {
                            type: "textarea",
                            rows: "4",
                            cols: "80",
                            defaultValue:
                              "Lamborghini Mercy, Your chick she so thirsty, I'm in that two seat Lambo.",
                            placeholder: "Here can be your description"
                          }
                        }
                      ]}
                    />
                  </form>
                </CardBody>
              </Card>
            </Col>
          </Row>
        </div>
      </div>
    );
  }
}

export default UserDetails;
