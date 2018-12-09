import * as React from "react";
import { FormGroup, Label, Input } from "reactstrap";

export interface Props {
  label?: string,
  formGroupProps?: any;
  labelProps?: any;
  inputProps?: any;
}

class CustomRadio extends React.Component<Props> {
  render() {
    var classes = "";
    if (this.props.formGroupProps !== undefined) {
      if (this.props.formGroupProps.className !== undefined) {
        classes += " " + this.props.formGroupProps.className;
      }
    }
    return (
      <FormGroup
        check
        {...this.props.formGroupProps}
        className={"form-check-radio" + classes}
      >
        <Label check {...this.props.labelProps}>
          <Input {...this.props.inputProps} type="radio" />
          <span className="form-check-sign" />
          {this.props.label ? this.props.label : ""}
        </Label>
      </FormGroup>
    );
  }
}

export default CustomRadio;
