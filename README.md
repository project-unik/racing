# Racing

A cross-platform racing game built with Unity 5.

__Assets__

We manage our Assets with an external SVN repository. The revision of
that SVN repository is tracked by Git. We use the [`git svnmodule`][gsvnm]
extension that was specifically developed for this purpose to better
integrate the SVN repository into the Git workflow.

[gsvnm]: https://github.com/NiklasRosenstein/git-svnmodule

After you clone the repository, it is important that you run
`git svnmodule init` to install the post-checkout hook.
