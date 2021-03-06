import * as React from "react";
import {
  FormGroup,
  Label,
  Input,
  InputGroup,
  InputGroupAddon
} from "reactstrap";


export interface FieldGroupProps {
  label: string
  addonLeft: any;
  addonRight: any;
  formGroupProps: any;
  labelProps: any;
  inputProps: any;
  inputGroupProps: any;
  inputGroupAddonProps: any;
}

interface State {
  focus: boolean;
}

class FieldGroup extends React.Component<FieldGroupProps, State> {
  constructor(props: FieldGroupProps) {
    super(props);
    this.state = {
      focus: false
    };
  }
  render() {
    const {
      label,
      addonLeft,
      addonRight,
      formGroupProps,
      labelProps,
      inputProps,
      inputGroupProps,
      inputGroupAddonProps
    } = this.props;
    var classes = " ";
    if (inputGroupProps !== undefined) {
      if (inputGroupProps.className !== undefined) {
        classes += inputGroupProps.className + " ";
      }
    }
    if (addonLeft !== undefined || addonRight !== undefined)
      return (
        <InputGroup
          {...inputGroupProps}
          className={classes + (this.state.focus ? "input-group-focus" : "")}
        >
          {addonLeft !== undefined ? (
            <InputGroupAddon {...inputGroupAddonProps}>
              {addonLeft}
            </InputGroupAddon>
          ) : (
            ""
          )}
          <Input
            {...inputProps}
            onFocus={e => this.setState({ focus: true })}
            onBlur={e => this.setState({ focus: false })}
          />
          {addonRight !== undefined ? (
            <InputGroupAddon {...inputGroupAddonProps}>
              {addonRight}
            </InputGroupAddon>
          ) : (
            ""
          )}
        </InputGroup>
      );
    return inputProps.type === "radio" || inputProps.type === "checkbox" ? (
      <FormGroup
        {...formGroupProps}
        className={inputProps.type === "radio" ? "form-check-radio" : ""}
      >
        <Label {...labelProps}>
          <Input {...inputProps} />
          <span className="form-check-sign" />
          {label ? label : ""}
        </Label>
      </FormGroup>
    ) : (
      <FormGroup {...formGroupProps}>
        {label ? <Label {...labelProps}>{label}</Label> : ""}
        <Input {...inputProps} />
      </FormGroup>
    );
  }
}

export interface FormInputProps {
  ncols: any[];
  proprieties: any[]
}

export class FormInputs extends React.Component<FormInputProps> {
  render() {
    var row = [];
    for (var i = 0; i < this.props.ncols.length; i++) {
      row.push(
        <div key={i} className={this.props.ncols[i]}>
          <FieldGroup {...this.props.proprieties[i]} />
        </div>
      );
    }
    return <div className="row">{row}</div>;
  }
}

export default FormInputs;
