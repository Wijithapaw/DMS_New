import * as React from "react";

export interface Props {
  children: any
}

class CardCategory extends React.Component<Props> {
  render() {
    return <h5 className="card-category">{this.props.children}</h5>;
  }
}

export default CardCategory;
