import * as React from "react";

export interface Props {
  size?: string;
  content?: any;
}

class PageHeader extends React.Component<Props> {
  render() {
    return (
      <div
        className={
          "panel-header " +
          (this.props.size !== undefined
            ? "panel-header-" + this.props.size
            : "")
        }
      >
        {this.props.content}
      </div>
    );
  }
}

export default PageHeader;
