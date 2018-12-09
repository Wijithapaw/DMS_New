import * as React from "react";
import { FormGroup, Label, Input } from "reactstrap";

export interface Props {
  label?: string,
  formGroupProps?: any;
  labelProps?: any;
  inputProps?: any;
}

class SimpleCheckbox extends React.Component<Props> {
  render() {
    return (
      <FormGroup check {...this.props.formGroupProps}>
        <Label check {...this.props.labelProps}>
          <Input type="checkbox" {...this.props.inputProps} />
          <span className="form-check-sign" />
          {this.props.label ? this.props.label : ""}
        </Label>
      </FormGroup>
    );
  }
}

export default SimpleCheckbox;
